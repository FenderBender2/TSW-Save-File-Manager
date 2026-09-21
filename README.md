# TSW-Save-File-Manager 1.2.1

A lightweight Windows utility for managing, backing up, restoring and organising save files for **Train Sim World** (TSW 3 to 6 so far).  
Automatically detects installed TSW versions and provides fast switching between multiple save slots.

---

## ✨ Features
- Auto‑detects installed TSW versions via Steam manifests  
- Supports multiple Steam library folders
- Supports multiple profiles    
- Clean UI with live update when save file changes  
- Fully portable EXE — no installation required
- Create folders to improve save file management  

---

## 📁 Supported TSW Versions
- Train Sim World 3  
- Train Sim World 4  
- Train Sim World 5  
- Train Sim World 6  
- Train Sim World 7  
- ...
---

## 🔍 How It Works
- Scans Steam’s `libraryfolders.vdf`  
- Reads each `appmanifest_*.acf`  
- Locates the TSW install folder  
- Watches the active save file for changes  

---

## 📦 Download
Grab the latest version from the **Releases** page:

👉 https://github.com/FenderBender2/TSW-Save-File-Manager/releases

---

## 🖼 Screenshots
<img width="519" height="618" alt="image" src="https://github.com/user-attachments/assets/7480510f-3eaf-4842-9d2f-858833ebb47f" />

---

## 🛠 Requirements
- Windows 10 or 11  
- .NET Framework 4.8 or .NET 6+ (depending on your build)  
- Steam installation of Train Sim World  

---

## 📚 Installation
1. Download the ZIP from Releases  
2. Extract anywhere  
3. Run `TSW Save File Manager.exe`  

No installer. No registry changes. Fully portable.

---

## 🧩 Known Limitations
- TSW save files cannot be decoded (UE4 binary format)
- Instead use descriptive save file names for routes, locos etc.
- Requires Steam installation paths to be intact  

---

## 📝 License
This project is licensed under the MIT License — see the **LICENSE** file.

---

## ❤️ Credits
Created by FenderBender2  
TSW community support and testing  
