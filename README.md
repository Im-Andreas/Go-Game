# Poor Man's Go

A sophisticated implementation of the ancient board game Go (also known as Weiqi or Baduk) built using C# Windows Forms with a focus on clean architecture, design patterns, and optimized performance.

## 🎮 About the Game

Go is a strategic board game originating from ancient China, played on a grid with black and white stones. Players take turns placing stones on empty intersections, with the goal of controlling territory and capturing opponent stones. This implementation features authentic Go rules including:

- **Territory Control**: Score based on controlled empty spaces
- **Stone Capture**: Remove opponent groups with no liberties
- **Ko Rule**: Prevents immediate recapture
- **Pass Mechanism**: End game when both players pass consecutively

## 🏗️ Architecture & Design Patterns

### Core Design Patterns

#### 1. **Proxy Pattern**
- **`PlayerWindow_Safe`**: A proxy class that wraps `PlayerWindow` to prevent unwanted external modifications
- Provides controlled access to main window components from game logic classes
- Ensures encapsulation and maintains separation of concerns

```csharp
/// <summary>
/// Proxy class made solely to prevent any (unwanted) modifications to
/// the main window's components from outside its own scope.
/// </summary>
public class PlayerWindow_Safe
{
    private readonly PlayerWindow wrappedWindow;
    // Controlled access methods...
}
```

#### 2. **Strategy Pattern**
- **`CellPos` and `CellPos_ExtraPrecision`**: Different coordinate handling strategies
- Polymorphic behavior for position calculations and area detection

#### 3. **Singleton-like Pattern**
- **Static direction arrays**: Reusable directional movement patterns in `Go_Board` and `Go_String`
- Optimized memory usage for common game operations

#### 4. **Observer Pattern**
- Event-driven UI updates for score changes and game state
- Real-time visual feedback for stone placement and captures

### Class Hierarchy

```
Go_Game/
├── UI Layer
│   ├── MainMenuWindow          # Game launcher with board size selection
│   ├── PlayerWindow            # Main game interface
│   └── PlayerWindow_Safe       # Proxy for controlled access
├── Game Logic Layer
│   ├── Go_Board                # Board state and game rules
│   ├── Go_String               # Stone groups and capture logic
│   └── CellPos                 # Position handling
└── Utility Layer
    ├── Point_IEqualityComparer # Optimized Point comparison
    └── Enums                   # Game state definitions
```

## 🎯 Key Features

### 🎨 Modern UI Design
- **Dark Theme**: Professional appearance with custom color schemes
- **Responsive Layout**: Adaptive scaling for different window sizes
- **Smooth Animations**: Gradient backgrounds and hover effects
- **Custom Controls**: Styled buttons and labels with rounded corners

### 🧠 Advanced Game Logic
- **String Management**: Efficient handling of connected stone groups (Go_String)
- **Liberty Calculation**: Real-time tracking of breathing spaces for stone groups
- **Territory Scoring**: Automatic calculation of controlled areas
- **Capture Detection**: Instant removal of captured stone groups

### ⚡ Performance Optimizations
- **Custom Hash Functions**: Optimized `Point_IEqualityComparer` for faster HashSet operations
- **Cached Rendering**: Wood grain pattern caching for improved drawing performance
- **Efficient Collections**: Strategic use of HashSet and List for optimal lookup times
- **Memory Management**: Proper disposal of graphics resources

### 📏 Multiple Board Sizes
- **Small (9×9)**: Perfect for quick games
- **Medium (13×13)**: Balanced gameplay
- **Historical (17×17)**: Traditional intermediate size
- **Normal (19×19)**: Professional tournament standard

## 🛠️ Technical Implementation

### Core Technologies
- **.NET Framework 4.7.2**
- **Windows Forms** for UI
- **GDI+** for custom graphics rendering
- **C# 7.3** language features

### Data Structures
- **Jagged Arrays**: Efficient board representation with `int[][]`
- **HashSet<Point>**: Fast stone and liberty tracking
- **Generic Collections**: Type-safe stone group management
- **Enumerators**: Memory-efficient iteration over stone collections

### Graphics Engine
- **Anti-aliased Rendering**: Smooth stone and board graphics
- **Gradient Brushes**: Realistic wood grain board texture
- **Path-based Drawing**: Rounded UI elements
- **Custom Painting**: Override paint events for enhanced visuals

## 🎲 Game Rules Implementation

### Stone Placement
- Validates legal moves before placement
- Prevents suicide moves
- Implements Ko rule to prevent infinite loops

### Scoring System
- **Area Scoring**: Points for controlled territory plus stones on board
- **Real-time Updates**: Live score tracking during gameplay
- **Capture Bonus**: Automatic score adjustment for captured stones

### Game Flow
- Turn-based gameplay with visual player indicators
- Pass mechanism with automatic game end detection
- Victory conditions with detailed score breakdown

## 🚀 Getting Started

### Prerequisites
- Windows OS (7/8/10/11)
- .NET Framework 4.7.2 or higher
- Visual Studio 2017+ (for development)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Im-Andreas/Go-Game.git
   ```
2. Open `Go_Game.sln` in Visual Studio
3. Build and run the solution (F5)

### How to Play
1. Launch the application
2. Select your preferred board size from the dropdown
3. Click "Start Game" to begin
4. Take turns placing black and white stones
5. Use the "Pass" button when you cannot make beneficial moves
6. Game ends when both players pass consecutively

## 🔧 Development Notes

### Code Organization
- **Namespace**: `Go_Game` - Contains all project classes
- **Enums**: Centralized in `Program.cs` for global access
- **Separation of Concerns**: Clear distinction between UI, game logic, and utilities

### Performance Considerations
- **Optimized Comparisons**: Custom `Point_IEqualityComparer` with bit-shift hashing
- **Lazy Evaluation**: Territory calculation only when needed
- **Resource Management**: Proper disposal of graphics objects using `using` statements

### Extensibility
- **Modular Design**: Easy to add new board sizes or rule variants
- **Event System**: Simple to add new UI feedback mechanisms
- **Proxy Pattern**: Safe extension of window functionality

## 🎨 Visual Design Philosophy

The application embraces a modern, professional aesthetic with:
- **Dark UI Theme**: Reduces eye strain during extended play
- **Subtle Animations**: Enhances user experience without distraction
- **Authentic Board Appearance**: Realistic wood grain texture with proper Go board proportions
- **Intuitive Controls**: Clear visual feedback for all interactive elements

