# Unity Scene Setup Guide

Step-by-step guide to create the UI scene for your Karaoke app.

---

## 📋 Overview

This guide will help you create the complete UI hierarchy for the karaoke app in Unity.

**Estimated Time:** 30-45 minutes  
**Difficulty:** Beginner-Friendly

---

## 🎨 Scene Hierarchy

```
KaraokeScene
├── Main Camera
├── EventSystem
├── Canvas
│   ├── SearchPanel
│   │   ├── Background
│   │   ├── TitleText
│   │   ├── SearchContainer
│   │   │   ├── SearchInputField
│   │   │   └── SearchButton
│   │   ├── ResultsScrollView
│   │   │   ├── Viewport
│   │   │   │   └── ResultsContainer
│   │   │   └── Scrollbar
│   │   └── LoadingIndicator
│   │
│   └── PlaybackPanel
│       ├── Background
│       ├── LyricsContainer
│       │   ├── PreviousLyricText
│       │   ├── CurrentLyricText
│       │   └── NextLyricText
│       │
│       └── ControlsContainer
│           ├── ButtonsRow
│           │   ├── PlayButton
│           │   ├── PauseButton
│           │   └── StopButton
│           │
│           ├── ProgressContainer
│           │   ├── CurrentTimeText
│           │   ├── ProgressSlider
│           │   └── TotalTimeText
│           │
│           └── VolumeContainer
│               ├── VolumeIcon
│               └── VolumeSlider
│
└── KaraokeManager (Empty GameObject)
    ├── KaraokeManager (Script)
    ├── LyricsDisplay (Script)
    ├── SearchUI (Script)
    ├── PlaybackController (Script)
    ├── AndroidPermissions (Script)
    └── AudioSource (Component)
```

---

## 🔨 Step-by-Step Creation

### Step 1: Create New Scene

1. File → New Scene
2. Save as "KaraokeScene"
3. Ensure you have:
   - Main Camera
   - EventSystem

---

### Step 2: Create Canvas

1. Right-click in Hierarchy → UI → Canvas
2. Set Canvas properties:
   - **Render Mode:** Screen Space - Overlay
   - **Canvas Scaler:**
     - UI Scale Mode: Scale With Screen Size
     - Reference Resolution: 1080 x 1920 (portrait)
     - Match: 0.5

---

### Step 3: Create Search Panel

#### 3.1 Search Panel Container

1. Right-click Canvas → UI → Panel
2. Rename to "SearchPanel"
3. Set RectTransform:
   - Anchor: Top stretch
   - Pos Y: 0
   - Height: 800

#### 3.2 Title Text

1. Right-click SearchPanel → UI → Text - TextMeshPro
2. Rename to "TitleText"
3. Configure:
   - Text: "Karaoke Search"
   - Font Size: 48
   - Alignment: Center
   - Color: White
4. Position at top of panel

#### 3.3 Search Input Field

1. Right-click SearchPanel → UI → Input Field - TextMeshPro
2. Rename to "SearchInputField"
3. Configure:
   - Placeholder Text: "Search for a song..."
   - Font Size: 32
4. Set RectTransform:
   - Width: 700
   - Height: 80

#### 3.4 Search Button

1. Right-click SearchPanel → UI → Button - TextMeshPro
2. Rename to "SearchButton"
3. Configure button text:
   - Text: "Search"
   - Font Size: 32
4. Set RectTransform:
   - Width: 200
   - Height: 80

#### 3.5 Results Scroll View

1. Right-click SearchPanel → UI → Scroll View
2. Rename to "ResultsScrollView"
3. Configure:
   - Delete Horizontal Scrollbar
   - Keep Vertical Scrollbar
4. Select "Content" child:
   - Add Component: Vertical Layout Group
     - Spacing: 10
     - Child Force Expand: Width = true, Height = false
   - Add Component: Content Size Fitter
     - Vertical Fit: Preferred Size
5. Rename "Content" to "ResultsContainer"

#### 3.6 Loading Indicator

1. Right-click SearchPanel → UI → Image
2. Rename to "LoadingIndicator"
3. Configure:
   - Add a loading spinner sprite
   - Set to inactive by default
4. Optional: Add rotation animation

---

### Step 4: Create Result Item Prefab

#### 4.1 Create Result Item

1. Right-click in Hierarchy → UI → Panel
2. Rename to "ResultItemPrefab"
3. Set RectTransform:
   - Width: 1000
   - Height: 150

#### 4.2 Add Thumbnail

1. Right-click ResultItemPrefab → UI → Raw Image
2. Rename to "ThumbnailImage"
3. Set RectTransform:
   - Anchor: Left
   - Width: 200
   - Height: 120

#### 4.3 Add Title Text

1. Right-click ResultItemPrefab → UI → Text - TextMeshPro
2. Rename to "TitleText"
3. Configure:
   - Font Size: 28
   - Alignment: Left
   - Overflow: Ellipsis
4. Position next to thumbnail

#### 4.4 Add Channel Text

1. Right-click ResultItemPrefab → UI → Text - TextMeshPro
2. Rename to "ChannelText"
3. Configure:
   - Font Size: 20
   - Color: Gray
   - Alignment: Left
4. Position below title

#### 4.5 Add Select Button

1. Right-click ResultItemPrefab → UI → Button - TextMeshPro
2. Rename to "SelectButton"
3. Configure:
   - Text: "Select"
   - Font Size: 24
4. Position on right side

#### 4.6 Add ResultItemUI Script

1. Select ResultItemPrefab
2. Add Component → ResultItemUI (script)
3. Assign references:
   - Title Text
   - Channel Text
   - Thumbnail Image
   - Select Button

#### 4.7 Save as Prefab

1. Create folder: Assets/Prefabs
2. Drag ResultItemPrefab to Prefabs folder
3. Delete from Hierarchy

---

### Step 5: Create Playback Panel

#### 5.1 Playback Panel Container

1. Right-click Canvas → UI → Panel
2. Rename to "PlaybackPanel"
3. Set RectTransform:
   - Anchor: Bottom stretch
   - Height: 1120
4. Set to inactive by default

#### 5.2 Lyrics Container

1. Right-click PlaybackPanel → Create Empty
2. Rename to "LyricsContainer"
3. Set RectTransform:
   - Anchor: Top stretch
   - Height: 600

#### 5.3 Previous Lyric Text

1. Right-click LyricsContainer → UI → Text - TextMeshPro
2. Rename to "PreviousLyricText"
3. Configure:
   - Font Size: 28
   - Color: Gray (128, 128, 128)
   - Alignment: Center
4. Position at top

#### 5.4 Current Lyric Text

1. Right-click LyricsContainer → UI → Text - TextMeshPro
2. Rename to "CurrentLyricText"
3. Configure:
   - Font Size: 42
   - Color: Yellow (255, 255, 0)
   - Alignment: Center
   - Font Style: Bold
4. Position in middle

#### 5.5 Next Lyric Text

1. Right-click LyricsContainer → UI → Text - TextMeshPro
2. Rename to "NextLyricText"
3. Configure:
   - Font Size: 28
   - Color: Gray (128, 128, 128)
   - Alignment: Center
4. Position at bottom

---

### Step 6: Create Playback Controls

#### 6.1 Controls Container

1. Right-click PlaybackPanel → Create Empty
2. Rename to "ControlsContainer"
3. Set RectTransform:
   - Anchor: Bottom stretch
   - Height: 400

#### 6.2 Buttons Row

1. Right-click ControlsContainer → Create Empty
2. Rename to "ButtonsRow"
3. Add Component: Horizontal Layout Group
   - Spacing: 20
   - Child Alignment: Middle Center

#### 6.3 Play Button

1. Right-click ButtonsRow → UI → Button - TextMeshPro
2. Rename to "PlayButton"
3. Configure:
   - Text: "▶" or "Play"
   - Font Size: 36
4. Set size: 120 x 120

#### 6.4 Pause Button

1. Right-click ButtonsRow → UI → Button - TextMeshPro
2. Rename to "PauseButton"
3. Configure:
   - Text: "⏸" or "Pause"
   - Font Size: 36
4. Set size: 120 x 120

#### 6.5 Stop Button

1. Right-click ButtonsRow → UI → Button - TextMeshPro
2. Rename to "StopButton"
3. Configure:
   - Text: "⏹" or "Stop"
   - Font Size: 36
4. Set size: 120 x 120

---

### Step 7: Create Progress Controls

#### 7.1 Progress Container

1. Right-click ControlsContainer → Create Empty
2. Rename to "ProgressContainer"
3. Add Component: Horizontal Layout Group
   - Spacing: 10

#### 7.2 Current Time Text

1. Right-click ProgressContainer → UI → Text - TextMeshPro
2. Rename to "CurrentTimeText"
3. Configure:
   - Text: "00:00"
   - Font Size: 24
   - Alignment: Center
4. Set width: 100

#### 7.3 Progress Slider

1. Right-click ProgressContainer → UI → Slider
2. Rename to "ProgressSlider"
3. Configure:
   - Min Value: 0
   - Max Value: 1
   - Value: 0
4. Add Event Triggers:
   - Pointer Down → PlaybackController.OnSliderDragStart
   - Pointer Up → PlaybackController.OnSliderDragEnd

#### 7.4 Total Time Text

1. Right-click ProgressContainer → UI → Text - TextMeshPro
2. Rename to "TotalTimeText"
3. Configure:
   - Text: "00:00"
   - Font Size: 24
   - Alignment: Center
4. Set width: 100

---

### Step 8: Create Volume Controls

#### 8.1 Volume Container

1. Right-click ControlsContainer → Create Empty
2. Rename to "VolumeContainer"
3. Add Component: Horizontal Layout Group
   - Spacing: 10

#### 8.2 Volume Icon

1. Right-click VolumeContainer → UI → Image
2. Rename to "VolumeIcon"
3. Add a speaker icon sprite
4. Set size: 50 x 50

#### 8.3 Volume Slider

1. Right-click VolumeContainer → UI → Slider
2. Rename to "VolumeSlider"
3. Configure:
   - Min Value: 0
   - Max Value: 1
   - Value: 1

---

### Step 9: Create KaraokeManager GameObject

#### 9.1 Create GameObject

1. Right-click in Hierarchy → Create Empty
2. Rename to "KaraokeManager"

#### 9.2 Add Scripts

1. Add Component → KaraokeManager
2. Add Component → LyricsDisplay
3. Add Component → SearchUI
4. Add Component → PlaybackController
5. Add Component → AndroidPermissions
6. Add Component → Audio Source

---

### Step 10: Assign References

#### 10.1 KaraokeManager References

Select KaraokeManager GameObject:

1. **Youtube API Key:** (paste your key)
2. **Audio Source:** Drag AudioSource component
3. **Lyrics Display:** Drag LyricsDisplay component
4. **Search UI:** Drag SearchUI component

#### 10.2 LyricsDisplay References

In LyricsDisplay component:

1. **Current Lyric Text:** Drag CurrentLyricText
2. **Next Lyric Text:** Drag NextLyricText
3. **Previous Lyric Text:** Drag PreviousLyricText
4. **Highlight Color:** Yellow (255, 255, 0)
5. **Normal Color:** Gray (128, 128, 128)

#### 10.3 SearchUI References

In SearchUI component:

1. **Search Input Field:** Drag SearchInputField
2. **Search Button:** Drag SearchButton
3. **Results Container:** Drag ResultsContainer
4. **Result Item Prefab:** Drag from Prefabs folder
5. **Scroll Rect:** Drag ResultsScrollView
6. **Loading Indicator:** Drag LoadingIndicator

#### 10.4 PlaybackController References

In PlaybackController component:

1. **Karaoke Manager:** Drag KaraokeManager component
2. **Play Button:** Drag PlayButton
3. **Pause Button:** Drag PauseButton
4. **Stop Button:** Drag StopButton
5. **Progress Slider:** Drag ProgressSlider
6. **Current Time Text:** Drag CurrentTimeText
7. **Total Time Text:** Drag TotalTimeText
8. **Volume Slider:** Drag VolumeSlider
9. **Audio Source:** Drag AudioSource component

---

## 🎨 Styling Tips

### Color Scheme

```
Background: #1a1a2e (Dark Blue)
Panel: #16213e (Darker Blue)
Accent: #0f3460 (Blue)
Highlight: #e94560 (Red/Pink)
Text: #ffffff (White)
Secondary Text: #808080 (Gray)
```

### Fonts

- **Title:** Bold, 48pt
- **Lyrics (Current):** Bold, 42pt
- **Lyrics (Other):** Regular, 28pt
- **Buttons:** Bold, 32pt
- **Body Text:** Regular, 24pt

### Spacing

- Panel Padding: 20px
- Element Spacing: 10-20px
- Button Padding: 15px

---

## ✅ Testing Your Scene

### 1. Check Hierarchy

- [ ] All objects named correctly
- [ ] Proper parent-child relationships
- [ ] No missing references

### 2. Check Components

- [ ] All scripts attached
- [ ] All references assigned
- [ ] No null references

### 3. Test in Editor

1. Press Play
2. Check if UI displays correctly
3. Test button clicks (should log to console)
4. Check text displays

---

## 🐛 Common Issues

### Issue: Text Not Visible

**Solution:**
- Check Canvas render mode
- Ensure text color is not transparent
- Check z-order of elements

### Issue: Buttons Not Clickable

**Solution:**
- Ensure EventSystem exists
- Check if other UI elements are blocking
- Verify button has Image component

### Issue: Scroll View Not Working

**Solution:**
- Check ScrollRect component settings
- Ensure Content has proper size
- Verify Viewport mask is enabled

### Issue: References Not Assigned

**Solution:**
- Drag and drop from Hierarchy
- Ensure object names match
- Check if objects are active

---

## 📱 Mobile Optimization

### Safe Area

For devices with notches:

```csharp
// Add to a UI script
void ApplySafeArea()
{
    Rect safeArea = Screen.safeArea;
    Vector2 anchorMin = safeArea.position;
    Vector2 anchorMax = anchorMin + safeArea.size;

    anchorMin.x /= Screen.width;
    anchorMin.y /= Screen.height;
    anchorMax.x /= Screen.width;
    anchorMax.y /= Screen.height;

    GetComponent<RectTransform>().anchorMin = anchorMin;
    GetComponent<RectTransform>().anchorMax = anchorMax;
}
```

### Touch-Friendly Sizes

- Minimum button size: 100x100 pixels
- Minimum touch target: 44x44 points
- Spacing between elements: 10-20 pixels

---

## 🎉 Completion

Your Unity scene is now complete and ready for testing!

**Next Steps:**
1. Save your scene
2. Add scene to Build Settings
3. Test in Unity Editor
4. Build for Android
5. Test on device

---

## 📚 Additional Resources

- [Unity UI Documentation](https://docs.unity3d.com/Packages/com.unity.ugui@latest)
- [TextMesh Pro Documentation](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html)
- [Canvas Scaler Guide](https://docs.unity3d.com/Manual/script-CanvasScaler.html)

---

**Scene setup complete! Ready to build your karaoke app! 🎤**

