# Persistent Plan: Add README to NuGet Packages

## Overview
This plan describes the steps to ensure all NuGet packages in a solution include a README file and are properly configured for NuGet.org. It is reusable for any .NET solution.

## Steps
1. **Create a Feature Branch**
   - Work within the context of the existing current git branch.

2. **Identify NuGet Projects**
   - List all projects in the solution that are packed and published as NuGet packages.

3. **README.md Creation**
   - For each NuGet project, create a `README.md` file in the project root.
   - Ensure each README contains a project description, usage instructions, and license.

4. **Update .csproj Files**
   - Add `<PackageReadmeFile>README.md</PackageReadmeFile>` to each `.csproj`.
   - Add `<None Include="README.md" Pack="true" PackagePath="" />` to ensure README is included in the package.

5. **Test Packaging**
   - Run `dotnet pack --no-build -o ../nupkg` for each project.
   - Confirm the README is included in the resulting `.nupkg` file.

6. **Leave Changes Local**
   - Do not commit or push changes until review is complete.

7. **Repeatable**
   - Use this plan for any other solution by following the same steps.

---

*This file can be copied to any solution and reused as a checklist for preparing NuGet packages with README files.*
