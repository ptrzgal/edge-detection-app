option casemap:none

; Export the function to make it visible outside the DLL
PUBLIC AsmSobelFunction

.code

; Sobel filter function for edge detection in an image
; Input parameters:
;   RCX - pointer to the input image (inputImage)
;   RDX - pointer to the output image (outputImage)
;   R8  - image width (width)
;   R9  - image height (height)
;
; Output parameters:
;   The output image is stored in the memory pointed to by RDX (outputImage) after applying the Sobel filter.
;
; Registers modified:
;   RAX, RBX, RCX, RDX, RSI, RDI, R10, R11, R12, R13, R14, R15 and the local stack space (64 bytes)
AsmSobelFunction PROC
    ; ----------------- Function Prologue -----------------
    ; Save non-volatile registers as per the Microsoft x64 calling convention
    push    rbp
    mov     rbp, rsp
    push    rbx
    push    rsi
    push    rdi
    push    r12
    push    r13
    push    r14
    push    r15
    ; Allocate 64 bytes for local variables (buffers, accumulators, etc.)
    sub     rsp, 64         

    ; ----------------- Initialize Input Parameters -----------------
    ; Function parameters (Microsoft x64 convention):
    ; RCX = inputImage, RDX = outputImage, R8 = width, R9 = height
    mov     rsi, rcx        ; rsi points to the input image
    mov     rdi, rdx        ; rdi points to the output image
    mov     rbx, r8         ; rbx holds the image width
    mov     r10, r9         ; r10 holds the image height

    ; ----------------- Clear the Borders of the Output Image -----------------

    ; Clear the top (first) row of the output image
    xor     rax, rax        ; rax = 0 (column index)
top_row_loop:
    cmp     rax, rbx        ; check if we reached the end of the row (rax >= width)
    jge     top_row_done
    mov     byte ptr [rdi + rax], 0 ; set pixel to 0 (black)
    inc     rax
    jmp     top_row_loop
top_row_done:

    ; Clear the bottom (last) row of the output image
    ; Calculate the offset for the bottom row: (height - 1) * width
    mov     rax, r10        ; rax = height
    dec     rax             ; rax = height - 1
    imul    rax, rbx        ; rax = (height - 1) * width
    lea     r11, [rdi + rax] ; r11 points to the beginning of the bottom row of the output image
    xor     rax, rax        ; rax = 0 (column index)
bottom_row_loop:
    cmp     rax, rbx        ; iterate over all columns in the bottom row
    jge     bottom_row_done
    mov     byte ptr [r11 + rax], 0 ; clear the pixel in the bottom row (using r11 as the row pointer)
    inc     rax
    jmp     bottom_row_loop
bottom_row_done:

    ; Clear the left column of the output image
    xor     rax, rax        ; rax = 0 (row index)
left_col_loop:
    cmp     rax, r10        ; check if we reached the end of the image (rax >= height)
    jge     left_col_done
    mov     rcx, rax
    imul    rcx, rbx        ; rcx = rax * width = offset for the current row
    mov     byte ptr [rdi + rcx], 0 ; clear the pixel in the left column of the current row
    inc     rax
    jmp     left_col_loop
left_col_done:

    ; Clear the right column of the output image
    xor     rax, rax        ; rax = 0 (row index)
right_col_loop:
    cmp     rax, r10        ; check if we reached the end of the image
    jge     right_col_done
    mov     rcx, rax
    imul    rcx, rbx        ; rcx = offset for the current row
    mov     rdx, rbx        ; rdx = width
    dec     rdx             ; rdx = width - 1 (last column)
    add     rcx, rdx        ; rcx = offset of the last pixel in the current row
    mov     byte ptr [rdi + rcx], 0 ; clear the pixel in the right column
    inc     rax
    jmp     right_col_loop
right_col_done:

    ; ----------------- Process the Interior of the Image -----------------
    ; Process rows from 1 to (height - 2), skipping the border rows
    mov     r11, 1         ; r11 = current row index (starting at 1)
outer_loop:
    mov     rax, r10
    dec     rax             ; rax = height - 1
    cmp     r11, rax
    jge     end_outer_loop ; if r11 >= height - 1, exit the outer loop

    ; Set pointers for three consecutive rows of the input image:
    ; - r11 - 1: row above the current row
    ; - r11    : current row
    ; - r11 + 1: row below the current row
    mov     rax, r11
    dec     rax
    imul    rax, rbx      ; compute offset for the row above
    add     rax, rsi      ; pointer to the row above = inputImage + offset
    mov     qword ptr [rbp - 8], rax  ; store pointer to the row above (local variable at rbp - 8)

    mov     rax, r11
    imul    rax, rbx      ; offset for the current row
    add     rax, rsi      ; pointer to the current row = inputImage + offset
    mov     qword ptr [rbp - 16], rax ; store pointer to the current row (local variable at rbp - 16)

    mov     rax, r11
    inc     rax
    imul    rax, rbx      ; offset for the row below
    add     rax, rsi      ; pointer to the row below = inputImage + offset
    mov     qword ptr [rbp - 24], rax ; store pointer to the row below (local variable at rbp - 24)

    ; Set pointer for the current row in the output image
    mov     rax, r11
    imul    rax, rbx
    add     rax, rdi      ; pointer = outputImage + (r11 * width)
    mov     qword ptr [rbp - 32], rax ; store pointer to the current output row (local variable at rbp - 32)

    push    r11           ; save the current row index on the stack

    ; ----------------- Inner Loop – Processing Columns -----------------
    ; Process columns from 1 to (width - 2), excluding the border columns
    mov     r12, 1        ; r12 = current column index (starting at 1)
inner_loop_start:
    mov     rax, rbx
    dec     rax           ; rax = width - 1
    cmp     r12, rax
    jge     end_inner_loop ; if r12 >= width - 1, exit the inner loop

    ; Initialize accumulators – local variables used in calculations
    ; [rbp - 40] and [rbp - 44] serve as accumulators,
    ; while [rbp - 48] stores the current column index (though it is not used further in calculations)
    mov     dword ptr [rbp - 48], r12d
    mov     dword ptr [rbp - 40], 0  ; accumulator 1
    mov     dword ptr [rbp - 44], 0  ; accumulator 2

    ; ----------------- Process the Neighborhood Pixels -----------------
    ; Process pixels from three rows – above, current, and below – in the neighborhood of the current pixel (column r12)
    ;
    ; Processing the row above (r11 - 1):
    mov     r13, qword ptr [rbp - 8] ; r13 = pointer to the row above
    mov     r14, r12
    dec     r14                   ; r14 = column index (r12 - 1) – left neighbor
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the left neighbor
    add     dword ptr [rbp - 40], r15d ; add the value to accumulator 1
    add     dword ptr [rbp - 44], r15d ; add the value to accumulator 2

    mov     r13, qword ptr [rbp - 8] ; r13 = pointer to the row above
    movzx   r15d, byte ptr [r13 + r12] ; load the pixel value from the center of the row above
    add     dword ptr [rbp - 44], r15d ; add the value to accumulator 2
    add     dword ptr [rbp - 44], r15d ; add the value again (double contribution)

    mov     r13, qword ptr [rbp - 8] ; r13 = pointer to the row above
    mov     r14, r12
    inc     r14                   ; r14 = column index (r12 + 1) – right neighbor
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the right neighbor
    sub     dword ptr [rbp - 40], r15d ; subtract the value from accumulator 1
    add     dword ptr [rbp - 44], r15d ; add the value to accumulator 2

    ; Processing the current row (r11):
    mov     r13, qword ptr [rbp - 16] ; r13 = pointer to the current row
    mov     r14, r12
    dec     r14                   ; left neighbor (r12 - 1)
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the left side
    add     dword ptr [rbp - 40], r15d ; add the value to accumulator 1 (double contribution)
    add     dword ptr [rbp - 40], r15d

    mov     r13, qword ptr [rbp - 16] ; r13 = pointer to the current row
    mov     r14, r12
    inc     r14                   ; right neighbor (r12 + 1)
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the right side
    sub     dword ptr [rbp - 40], r15d ; subtract the value from accumulator 1 (double contribution)
    sub     dword ptr [rbp - 40], r15d

    ; Processing the row below (r11 + 1):
    mov     r13, qword ptr [rbp - 24] ; r13 = pointer to the row below
    mov     r14, r12
    dec     r14                   ; left neighbor (r12 - 1)
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the left neighbor
    add     dword ptr [rbp - 40], r15d ; add the value to accumulator 1
    sub     dword ptr [rbp - 44], r15d ; subtract the value from accumulator 2

    mov     r13, qword ptr [rbp - 24] ; r13 = pointer to the row below
    movzx   r15d, byte ptr [r13 + r12] ; load the pixel value from the center of the row below
    sub     dword ptr [rbp - 44], r15d ; subtract the value from accumulator 2 (double contribution)
    sub     dword ptr [rbp - 44], r15d

    mov     r13, qword ptr [rbp - 24] ; r13 = pointer to the row below
    mov     r14, r12
    inc     r14                   ; right neighbor (r12 + 1)
    movzx   r15d, byte ptr [r13 + r14] ; load the pixel value from the right neighbor
    sub     dword ptr [rbp - 40], r15d ; subtract the value from accumulator 1
    sub     dword ptr [rbp - 44], r15d ; subtract the value from accumulator 2

    ; ----------------- Calculate the Final Filter Value -----------------
    ; Compute the sum of the absolute values of both accumulators (i.e., |accumulator1| + |accumulator2|)
    mov     eax, dword ptr [rbp - 40] ; move accumulator 1 into eax
    cmp     eax, 0
    jge     skip_abs1         ; if accumulator1 is non-negative, skip negation
    neg     eax               ; otherwise, negate to obtain the absolute value
skip_abs1:
    mov     edx, dword ptr [rbp - 44] ; move accumulator 2 into edx
    cmp     edx, 0
    jge     skip_abs2         ; if accumulator2 is non-negative, skip negation
    neg     edx               ; otherwise, negate to obtain the absolute value
skip_abs2:
    add     eax, edx          ; sum of absolute values = |accumulator1| + |accumulator2|
    cmp     eax, 255
    jbe     no_clamp          ; if the sum <= 255, leave it unchanged
    mov     eax, 255          ; otherwise, clamp the value to 255 (maximum pixel value)
no_clamp:
    ; ----------------- Write the Result -----------------
    ; Save the computed, clamped value to the corresponding pixel in the output image
    mov     r13, qword ptr [rbp - 32] ; r13 = pointer to the current output row
    mov     byte ptr [r13 + r12], al  ; store the result (value in al) in column r12 of the current row

    ; Move to the next column in the current row
    inc     r12
    jmp     inner_loop_start
end_inner_loop:
    ; Restore the saved row index (pop from the stack) and move to the next row
    pop     r11
    inc     r11
    jmp     outer_loop
end_outer_loop:

    ; ----------------- Function Epilogue -----------------
    ; Restore allocated local space and all saved registers
    add     rsp, 64
    pop     r15
    pop     r14
    pop     r13
    pop     r12
    pop     rdi
    pop     rsi
    pop     rbx
    pop     rbp
    ret
AsmSobelFunction ENDP

END