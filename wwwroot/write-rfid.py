from mfrc522 import SimpleMFRC522
import RPi.GPIO as GPIO
import sys

def write_rfid(text):
    reader = SimpleMFRC522()
    try:
        print("Approach the card to the reader to write")
        reader.write(text)
        print("Written successfully!")
    finally:
        GPIO.cleanup()

if __name__ == "__main__":
    if len(sys.argv) > 1:
        text_to_write = sys.argv[1]
        write_rfid(text_to_write)
    else:
        print("Error: No text provided")
