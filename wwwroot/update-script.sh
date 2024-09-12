#!/bin/bash

# آدرس مخزن GitHub شما
GITHUB_REPO="https://github.com/SpCoreProject/SPBHashing"
LOCAL_VERSION_FILE="/var/www/html/Main/version.txt"
DOWNLOAD_DIR="/tmp/SPBHashing"
TARGET_DIR="/var/www/html/Main/"

# دریافت آخرین نسخه از گیت هاب
LATEST_VERSION=$(curl -s https://api.github.com/repos/SpCoreProject/SPBHashing/releases/latest | grep 'tag_name' | cut -d\" -f4)

# بررسی وجود نسخه محلی
if [ ! -f "$LOCAL_VERSION_FILE" ]; then
    echo "No local version found. Assuming first run."
    LOCAL_VERSION=""
else
    LOCAL_VERSION=$(cat $LOCAL_VERSION_FILE)
fi

# مقایسه نسخه‌ها
if [ "$LATEST_VERSION" != "$LOCAL_VERSION" ]; then
    echo "New version found: $LATEST_VERSION. Updating..."
    
    # دانلود آخرین نسخه و اکسترکت کردن آن
    wget -qO- "$GITHUB_REPO/archive/refs/tags/$LATEST_VERSION.tar.gz" | tar xz -C /tmp

    # پاک کردن نسخه قدیمی
    sudo rm -rf $TARGET_DIR/*

    # کپی نسخه جدید به پوشه پروژه
    sudo cp -R $DOWNLOAD_DIR/* $TARGET_DIR/

    # به روز رسانی نسخه محلی
    echo $LATEST_VERSION > $LOCAL_VERSION_FILE

    # ریستارت کردن سرویس برنامه
    sudo systemctl restart kestrel-your-app.service

    echo "Update complete."
else
    echo "No new version found."
fi
