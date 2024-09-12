
document.addEventListener('DOMContentLoaded', () => {
    let currentKeyIndex = 0;
    let activeInput = null; // Variable to store the currently active input
    const virtualKeyboard = document.getElementById('virtualKeyboard');
    //const virtualnumberKeyboard = document.getElementById('virtualnumberKeyboard');
    const keys = document.querySelectorAll('.key');
    let isShiftActive = false;

    document.querySelectorAll('input').forEach(input => {
        input.addEventListener('focus', () => {
            activeInput = input; // Set the currently active input
        });

        input.addEventListener('keydown', (event) => {
            if (event.key === 'Enter') {
                console.log(activeInput);
                event.preventDefault();
                if (virtualKeyboard.classList.contains('active')) {
                    virtualKeyboard.classList.remove('active');
                } else {
                    virtualKeyboard.classList.add('active');
                }
            }
        });
    });

    virtualKeyboard.addEventListener('mousedown', (event) => {
        event.preventDefault();
    });

    virtualKeyboard.addEventListener('click', (event) => {
        if (event.target.classList.contains('key')) {
            const key = event.target;
            if (key.classList.contains('shift')) {
                isShiftActive = !isShiftActive;
                key.classList.toggle('active');
                updateKeys();
            } else if (key.classList.contains('backspace')) {
                if (activeInput) {
                    activeInput.value = activeInput.value.slice(0, -1);
                }
            } else if (key.classList.contains('space')) {
                if (activeInput) {
                    activeInput.value += ' ';
                }
            } else if (key.classList.contains('enter')) {
                if (activeInput) {
                    activeInput.value += '\n';
                }
            } else if (key.classList.contains('paste')) {
                if (activeInput) {
                    navigator.clipboard.readText().then(text => {
                        activeInput.value += text;
                    });
                }
            } else if (key.classList.contains('move-left')) {
                moveCursor(-1);
            } else if (key.classList.contains('move-right')) {
                moveCursor(1);
            } else if (key.classList.contains('close')) {
                virtualKeyboard.classList.remove('active');
                activeInput = null; // Clear active input
            } else {
                if (activeInput) {
                    activeInput.value += isShiftActive ? key.dataset.shift : key.dataset.normal;
                }
            }
        }
    });

    document.addEventListener('keydown', (event) => {
        if (virtualKeyboard.classList.contains('active')) {
            switch (event.key) {
                case 'ArrowUp':
                    navigateKeyboard('up');
                    break;
                case 'ArrowDown':
                    navigateKeyboard('down');
                    break;
                case 'ArrowLeft':
                    navigateKeyboard('left');
                    break;
                case 'ArrowRight':
                    navigateKeyboard('right');
                    break;
                case 'Enter':
                    if (activeInput) {
                        activeInput.value += '\n';
                    }
                    break;
                case 'Shift':
                    isShiftActive = !isShiftActive;
                    updateKeys();
                    break;
                case 'ArrowLeft':
                    moveCursor(-1);
                    break;
                case 'ArrowRight':
                    moveCursor(1);
                    break;
                default:
                    break;
            }
        }
    });

    function updateKeys() {
        keys.forEach(key => {
            const shiftChar = key.querySelector('.shift-char');
            if (shiftChar) {
                shiftChar.remove();
            }
            if (key.dataset.shift) {
                const shiftCharElement = document.createElement('span');
                shiftCharElement.classList.add('shift-char');
                shiftCharElement.textContent = key.dataset.shift;
                key.appendChild(shiftCharElement);
            }
        });
    }

    function navigateKeyboard(direction) {
        keys.forEach(key => key.classList.remove('highlight'));

        switch (direction) {
            case 'up':
                if (currentKeyIndex >= 12) currentKeyIndex -= 12;
                break;
            case 'down':
                if (currentKeyIndex < keys.length - 12) currentKeyIndex += 12;
                break;
            case 'left':
                if (currentKeyIndex > 0) currentKeyIndex -= 1;
                break;
            case 'right':
                if (currentKeyIndex < keys.length - 1) currentKeyIndex += 1;
                break;
        }

        keys[currentKeyIndex].classList.add('highlight');
        keys[currentKeyIndex].focus();
    }

    function moveCursor(direction) {
        if (activeInput) {
            const value = activeInput.value;
            const position = activeInput.selectionStart;

            if (direction === -1) {
                // Move cursor left by one word
                const leftPart = value.substring(0, position).trim().split(/\s+/);
                const wordCount = leftPart.length;
                if (wordCount > 1) {
                    const newPosition = value.lastIndexOf(' ', position - 2);
                    activeInput.setSelectionRange(newPosition + 1, newPosition + 1);
                }
            } else if (direction === 1) {
                // Move cursor right by one word
                const rightPart = value.substring(position).trim().split(/\s+/);
                if (rightPart.length > 1) {
                    const newPosition = value.indexOf(' ', position);
                    activeInput.setSelectionRange(newPosition + 1, newPosition + 1);
                }
            }
        }
    }

    keys[currentKeyIndex].classList.add('highlight');
    keys[currentKeyIndex].focus();

    updateKeys(); // Call this function once to add shift characters
});