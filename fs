; ==========================================
; AetherOS Micro-FileSystem (AetherFS)
; ==========================================

MAX_FILES equ 8
FILENAME_LEN equ 12
FILE_SIZE equ 64

struc FileEntry
    .name:   resb FILENAME_LEN
    .used:   resb 1
    .size:   resb 1
    .data:   resb FILE_SIZE
endstruc

file_table: times MAX_FILES * FileEntry_size db 0

init_filesystem:
    pusha
    ; Создадим файл по умолчанию №1 (Hello World Script)
    mov si, default_fn1
    mov di, file_table + FileEntry.name
    mov cx, 12
    rep movsb
    mov byte [file_table + FileEntry.used], 1
    mov byte [file_table + FileEntry.size], 24
    
    mov si, default_content1
    mov di, file_table + FileEntry.data
    mov cx, 24
    rep movsb

    ; Создадим файл по умолчанию №2 (Notes)
    mov bx, FileEntry_size
    mov si, default_fn2
    lea di, [file_table + bx + FileEntry.name]
    mov cx, 12
    rep movsb
    mov byte [file_table + bx + FileEntry.used], 1
    
    popa
    ret

save_file:
    ; AL = file index, DS:SI = buffer pointer
    pusha
    mov ah, FileEntry_size
    mul ah
    mov di, ax
    add di, file_table
    
    mov byte [di + FileEntry.used], 1
    add di, FileEntry.data
    mov cx, FILE_SIZE
    rep movsb
    popa
    ret

default_fn1 db 'prog1.bas   ', 0
default_content1 db 'PRINT Hello Aether OS!', 0
default_fn2 db 'readme.txt  ', 0
