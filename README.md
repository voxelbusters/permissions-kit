# 📲 Permissions Kit  

**Simplify cross-platform permission management in Unity applications**  

Permissions Kit is a robust permission management framework designed to streamline access control in **Unity** projects. With a unified API for **iOS** and **Android**, it removes the complexity of platform-specific implementations—allowing you to request, verify, and manage app permissions with ease and consistency.  

---

## ✨ Key Features  

### ✅ Unified API for Cross-Platform Permission Handling  
- A single, streamlined API to manage permissions on **iOS** and **Android**.  
- Reduces platform-specific permission handling complexity.  

### 📊 Comprehensive Permission Support  
Permissions Kit supports a wide range of permissions across both platforms:  

- **Network & Connectivity**: Internet access, network state, Wi-Fi, and local network permissions.  
- **Location Services**: Fine and coarse location tracking, including background access.  
- **Notifications**: Push notification permissions with user consent management.  
- **Media Access**: Read and write permissions for images, videos, and audio files.  
- **Hardware Access**: Camera, microphone, Bluetooth, and vibration control.  

### 🔧 Automated Configuration for iOS and Android  
- **iOS**: Automatically adds the required usage descriptions in `Info.plist`.  
- **Android**: Automatically updates `AndroidManifest.xml` with necessary permissions, features, and hardware requirements.  
- **No Manual Setup Required**: Permissions Kit takes care of all necessary configurations for both platforms.  

### 🛠️ Simple and Efficient API  
Permissions Kit offers a developer-friendly API for requesting and checking permissions:  

```csharp
using VoxelBusters.PermissionsKit;
using VoxelBusters.CoreLibrary;

// Create a PermissionRequest instance
PermissionRequest request = new PermissionRequest.Builder()
                                .AddPermission(Permission.USE_CAMERA)
                                .AddPermission(Permission.PUSH_NOTIFICATIONS)
                                .ShowApplicationSettingsIfDeniedOrRestricted(true)
                                .Build();

// Request permissions
PermissionsKit.Request(request, (PermissionRequestResult result, Error error) =>
{
    if (result.Authorized())
    {
        Debug.Log("✅ Permissions granted!");
    }
    else
    {
        Debug.Log("🚫 Permissions denied.");
    }
});

// Check the status of a specific permission
PermissionStatus status = PermissionsKit.GetStatus(Permission.RECORD_AUDIO);
```
### 📈 Advanced Permission Management for Better DX (Developer Experience)

**Smart Handling**: Prevents unnecessary prompts if permissions are already granted.  
**Development Mode Alerts**: Warns developers about missing configurations during development.  
**User-Friendly Experience**:  
- Displays helpful alerts if a user denies a permission.  
- Provides an option to open the app’s settings to manually enable permissions.  

---

## 🚀 Getting Started

### 📥 Installation
To install **Permissions Kit** in your Unity project, follow these steps:

1. Go to the **Releases** section of the repository.
2. Download the latest `.unitypackage` file.
3. Open your Unity project.
4. Drag and drop the downloaded `.unitypackage` into your project, or use **Assets > Import Package > Custom Package** to import it.
5. Enable the permissions you want to use in Window -> Voxel Busters -> Permissions Kit settings

**Permissions Kit** is now ready to use in your project.

---

## 📊 Supported Permissions
Here are the key permission categories supported by **Permissions Kit**:

| Category        | Permissions                                           |
|-----------------|-------------------------------------------------------|
| 📡 Network      | Internet, Network State, Wi-Fi, Local Network         |
| 📍 Location     | Fine Location, Coarse Location, Background            |
| 🔔 Notifications| Push Notifications, Consent Management               |
| 📸 Media Access| Camera, Audio Recording, Image/Video Access           |
| 🔌 Hardware     | Microphone, Bluetooth, Vibration Control              |

---

## 📋 Permission Status
**Permissions Kit** supports the following statuses:

- **Unknown**: Permission status is unknown or not yet requested.
- **Granted**: Permission is approved by the user.
- **Limited**: Permission is approved by the user but with limited access.
- **Restricted**: System-level policies restrict access.
- **Denied**: User has denied the permission.

---
### **Unlock More Features with [Essential Kit](https://link.voxelbusters.com/essential-kit)**

While Permissions Kit efficiently manages app permissions, if you need access to additional native features such as:

-   **Local and Remote Notifications**
    
-   **In-App Purchases (Consumables, Non-Consumables & Subscriptions)**
    
-   **WebView**
    
-   **Leaderboards & Achievements**
    
-   **Cloud Save**
    
-   **Task Services (Allow app in background)**
    
-   **App Updater**
    
-   **App Review**
    
-   **Deep Links**
    
-   **Sharing**
    
-   **And More**
    

Explore [**Essential Kit**](https://link.voxelbusters.com/essential-kit) to enhance your Unity development experience with a powerful suite of features tailored for mobile game developers.

---

## 🤝 Contribution
We welcome contributions to **Permissions Kit**! If you'd like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch (`feature/my-feature`).
3. Commit your changes.
4. Open a pull request with a detailed description of your changes.

---

## 📄 License
**Permissions Kit** is licensed under the **MIT License** – see the LICENSE file for details.

---

## 📧 Support
For questions or support, feel free to open an issue or contact us via **Voxel Busters**.

---

**🛠️ Simplify your permission management with Permissions Kit – Built for Unity!**
