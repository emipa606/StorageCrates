# GitHub Copilot Instructions for Storage Crates (Continued) Mod

Welcome to the GitHub Copilot Instructions for the Storage Crates (Continued) mod. This document provides an overview of the mod, its key features and systems, coding patterns and conventions, XML integration, Harmony patching, and suggestions for using Copilot effectively.

## Mod Overview and Purpose

Storage Crates (Continued) is a mod focused on compact and neat stockpiling of bulk resources in the RimWorld game. It allows players to build specialized crates that can store large quantities of resources without taking up much space in their stockpiles. The mod introduces various crates and packing benches to facilitate the process of creating and using these crates.

## Key Features and Systems

### Crates
- **Steel Crates**: Built using steel crate plates and a full stack of resources, allowing for efficient storage of up to 75 units of a non-currency resource per crate.
- **Gold and Silver Crates**: Similar to steel crates but with a higher capacity of 500 units per crate.
- **Processed Meat Crates**: Store processed meat in large quantities, reducing the need for multiple stacks.
- **Meal Cans**: Canned meat and vegetable meals that do not spoil, which can then be packed into crates for long-term storage.

### Packing Benches
- **Packing Bench Tier One**: Used to create steel crate plates from raw resources.
- **Packing Bench Tier Two**: Used to assemble crates from crate plates and full stacks of resources.
- **Food Bench**: Used to create meal cans from raw resources and butchered meat or vegetables.

### XML Integration
The mod uses XML files to define various game elements such as recipes, skills, and other game definitions. Key XML files include:
- `RecipeDefs/Canning_Recipes.xml`
- `RecipeDefs/Packing_Recipes.xml`
- `RecipeDefs/SteelPlates.xml`
- `RecipeDefs/Unpacking_Recipes.xml`
- `StatDefs/Packing_Skills.xml`

### Harmony Patching
The mod uses Harmony patching to modify existing game behaviors. This includes:
- Patching recipes to ensure they work correctly with the new crates and packing benches.
- Modifying game mechanics to support the unique properties of the new crates.

## Coding Patterns and Conventions

### C# Files
The mod's C# code is organized into several files, each responsible for different aspects of the mod. Key files include:
- **FoodBench.cs**: Handles the functionality of the food bench, including recipe creation and gizmo management.
- **Crate.cs**: Manages the creation and behavior of crates, including reading from XML and handling spawning.
- **KompressionModThingDefs.cs**: Contains definitions for various mod-specific things.
- **PackingBenchTierOne.cs**: Handles the functionality of the packing bench tier one, including recipe creation and gizmo management.

### XML Files
The mod's XML files are used to define game elements. Key XML files include:
- `RecipeDefs/Canning_Recipes.xml`: Defines recipes for creating meal cans.
- `RecipeDefs/Packing_Recipes.xml`: Defines recipes for creating various crates.
- `RecipeDefs/SteelPlates.xml`: Defines recipes for creating steel plates.
- `RecipeDefs/Unpacking_Recipes.xml`: Defines recipes for unpacking crates.
- `StatDefs/Packing_Skills.xml`: Defines skills related to packing and crate creation.

## Suggestions for Copilot

### General Suggestions
- **Use Descriptive Comments**: When writing code, use descriptive comments to explain the purpose of functions, classes, and complex logic.
- **Follow Coding Standards**: Adhere to established coding standards, including naming conventions and code structure.
- **Write Unit Tests**: Write unit tests for your code to ensure that it behaves as expected and to facilitate future development.

### Specific Suggestions for Storage Crates (Continued)
- **XML Definitions**: When working with XML files, ensure that your definitions are accurate and complete. Use Copilot to generate and validate XML snippets.
- **Harmony Patching**: When creating Harmony patches, ensure that your patches do not break existing game mechanics. Use Copilot to generate and validate patch code.
- **C# Code**: When writing C# code, ensure that your code is efficient and maintainable. Use Copilot to generate and validate code snippets.

## Conclusion

The Storage Crates (Continued) mod is a powerful tool for compact and efficient resource storage in RimWorld. By following the coding patterns and conventions outlined in this document, and by leveraging the suggestions for using Copilot, you can contribute to the continued development and improvement of this mod. Thank you for your contributions!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

