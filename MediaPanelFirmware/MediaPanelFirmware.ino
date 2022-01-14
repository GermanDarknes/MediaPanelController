#define HID_CUSTOM_LAYOUT
#define LAYOUT_US_ENGLISH

#define buttonCount 5
#define lcdAddress 0x27
#define lcdCountLetters 16
#define lcdCountLines 2

#include <Wire.h>
#include <HID-Project.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(lcdAddress, lcdCountLetters, lcdCountLines);

int keyPin[buttonCount] = {8, 7, 6, 5, 4};
int keyCode[buttonCount] = {MEDIA_VOLUME_UP, MEDIA_VOLUME_DOWN, MEDIA_PREVIOUS, MEDIA_PLAY_PAUSE, MEDIA_NEXT};
bool keyState[buttonCount] = {false, false, false, false, false};

int seperatorPosition, seperatorOffset;
int scrollCounter = 0;
int scrollStatusLine[lcdCountLines] = { 0 };
String readMessage = "";
String textLine[lcdCountLines] = {""};


void toggleKey(int index) {
  if (digitalRead(keyPin[index]) == HIGH && keyState[index]) {
    keyState[index] = false;
    Consumer.release(keyCode[index]);
  }
  else if (digitalRead(keyPin[index]) == LOW && !keyState[index]) {
    keyState[index] = true;
    Consumer.press(keyCode[index]);
  }
}

void initialPrintLine(String text, int lineIndex) {
    if(text.length() > lcdCountLetters) {
      lcd.setCursor(0, lineIndex);
      lcd.print(text.substring(0, lcdCountLetters));
    }
    else {
      int spacing = (lcdCountLetters - text.length()) / 2;

      lcd.setCursor(spacing, lineIndex);
      lcd.print(text);
    }
}

void shiftPrintLine(String text, int lineIndex) {
    if(text.length() > lcdCountLetters) {
      lcd.setCursor(0, lineIndex);

      String newText = text.substring(scrollStatusLine[lineIndex]);

      lcd.print(newText.substring(0, 16));
      ++scrollStatusLine[lineIndex];

      if(newText.length() == 16) {
        scrollStatusLine[lineIndex] = 0;
      }
    }
}

void setup() {
  for (int i = 0; i < buttonCount; i++)  {
    pinMode(keyPin[i], INPUT_PULLUP);
  }

  lcd.init();
  lcd.backlight();
  analogWrite(A0, 255);

  Serial.begin(115200);
  Consumer.begin();

  initialPrintLine("Hallo Martin", 0);
  initialPrintLine("", 1);
}

void loop() {
  for (int i = 0; i < buttonCount; i++)  {
    toggleKey(i);
  }

  while (Serial.available()) {
    delay(10);  
    if (Serial.available() > 0) {
      char c = Serial.read();
      readMessage += c;
    }
  }

  if (readMessage.length() > 0) {
    if (readMessage.equals("whoru")) {
      Serial.println("MediaPanel");
    }
    else {
      seperatorOffset = 0;
      seperatorPosition = readMessage.indexOf(';');

      for(int i = 0; i < lcdCountLines; i++) {
        String currentText = readMessage.substring(0, seperatorPosition);
        if (currentText.length() > 1) {
          textLine[i] = readMessage.substring(0, seperatorPosition);
        }
        else {
          textLine[i] = "";
        }

        readMessage = readMessage.substring(seperatorPosition+1);
        seperatorPosition = readMessage.indexOf(';');
      }

      lcd.clear();

      for(int i = 0; i < lcdCountLines; i++) {
        initialPrintLine(textLine[i], i);
        scrollStatusLine[i] = 0;
      }

      scrollCounter = 0;
    }
    
    readMessage = "";
  }
  else if(scrollCounter == 100) {
    for(int i = 0; i < lcdCountLines; i++) {
      shiftPrintLine(textLine[i], i);
    }
    scrollCounter = 0;
  }

  delay(10); 

  ++scrollCounter;
}