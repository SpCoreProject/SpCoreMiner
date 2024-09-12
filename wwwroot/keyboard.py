import uinput
import time

# ایجاد دستگاه ورودی مجازی
events = (
    uinput.KEY_TAB,
    uinput.KEY_UP,
    uinput.KEY_DOWN,
    uinput.KEY_LEFT,
    uinput.KEY_RIGHT,
    uinput.KEY_1,
    uinput.KEY_2,
    uinput.KEY_3,
    uinput.KEY_4,
    uinput.KEY_5,
    uinput.KEY_6,
    uinput.KEY_7,
    uinput.KEY_8,
    uinput.KEY_9,
    uinput.KEY_0,
    uinput.KEY_ENTER
)

device = uinput.Device(events)

# دکمه‌هایی که باید شبیه‌سازی شوند
key_map = {
    'KEY_TAB': uinput.KEY_TAB,
    'KEY_UP': uinput.KEY_UP,
    'KEY_DOWN': uinput.KEY_DOWN,
    'KEY_LEFT': uinput.KEY_LEFT,
    'KEY_RIGHT': uinput.KEY_RIGHT,
    'KEY_1': uinput.KEY_1,
    'KEY_2': uinput.KEY_2,
    'KEY_3': uinput.KEY_3,
    'KEY_4': uinput.KEY_4,
    'KEY_5': uinput.KEY_5,
    'KEY_6': uinput.KEY_6,
    'KEY_7': uinput.KEY_7,
    'KEY_8': uinput.KEY_8,
    'KEY_9': uinput.KEY_9,
    'KEY_0': uinput.KEY_0,
    'KEY_ENTER': uinput.KEY_ENTER
}

def press_key(key):
    if key in key_map:
        key_to_press = key_map[key]
        try:
            device.emit(key_to_press, 1)  # 1 برای فشار دادن کلید
            time.sleep(0.1)  # تاخیر برای شبیه‌سازی فشار کلید
            device.emit(key_to_press, 0)  # 0 برای آزاد کردن کلید
        except Exception as e:
            print(f"Error pressing key {key}: {e}")
    else:
        print(f"Key {key} not found in key_map")

# اجرای مداوم برنامه
def main():
    print("Starting key simulation. Press Ctrl+C to exit.")
    try:
        while True:
            #time.sleep(2)  # زمان برای تغییر به پنجره مرورگر
            press_key('KEY_TAB')
           # time.sleep(0.5)
            press_key('KEY_UP')
           # time.sleep(0.5)
            press_key('KEY_DOWN')
           # time.sleep(0.5)
            press_key('KEY_LEFT')
           # time.sleep(0.5)
            press_key('KEY_RIGHT')
           # time.sleep(0.5)
            press_key('KEY_1')
           # time.sleep(0.5)
            press_key('KEY_ENTER')
           # time.sleep(5)  # زمان انتظار قبل از تکرار حلقه
    except KeyboardInterrupt:
        print("Program interrupted by user. Exiting...")

if __name__ == "__main__":
    main()
