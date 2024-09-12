from mfrc522 import SimpleMFRC522
import RPi.GPIO as GPIO

def read_rfid():
    reader = SimpleMFRC522()

    try:
        print("Approach the card to the reader")
        id, text = reader.read()
        print(f"{id},{text.strip()}")
    finally:
        GPIO.cleanup()

if __name__ == "__main__":
    read_rfid()
