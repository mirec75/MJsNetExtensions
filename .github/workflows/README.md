# GitHub Actions Workflows

This directory contains GitHub Actions workflows for automating the build, test, and release process.

## Workflows

### 1. Build and Test (`build-and-test.yml`)

**Triggers:**
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches

**What it does:**
- ✅ Sets up .NET 8
- ✅ Installs and runs GitVersion
- ✅ Restores dependencies
- ✅ Builds the solution in Release configuration
- ✅ Runs all tests
- ✅ Uploads test results as artifacts
- ✅ Creates NuGet package (only on `main` branch pushes)
- ✅ Uploads NuGet package as artifact (only on `main` branch pushes)

### 2. Create Release (`release.yml`)

**Triggers:**
- Manual workflow dispatch (requires approval/manual trigger)

**What it does:**
- ✅ Sets up .NET 8
- ✅ Determines version using GitVersion (or accepts manual version input)
- ✅ Builds and tests the solution
- ✅ Creates NuGet package
- ✅ Creates and pushes Git tag (e.g., `v1.0.0`)
- ✅ Creates GitHub Release with:
  - Release notes
  - NuGet package attached
  - Installation instructions
- ✅ (Optional) Publishes to NuGet.org if API key is configured

## How to Use

### Running Build and Test

This workflow runs automatically on every push and pull request. No action needed!

### Creating a Release

1. **Go to GitHub Actions tab** in your repository
2. **Select "Create Release"** workflow from the left sidebar
3. **Click "Run workflow"** button
4. **Choose options:**
   - Select the branch (usually `main`)
   - Optionally enter a specific version (leave empty to use GitVersion)
5. **Click "Run workflow"** to start

The workflow will:
- Build and test the code
- Create a Git tag
- Create a GitHub Release with the NuGet package attached

### Publishing to NuGet.org

To enable automatic publishing to NuGet.org during releases:

1. **Get your NuGet API key:**
   - Go to [nuget.org](https://www.nuget.org/)
   - Sign in and go to "API Keys"
   - Create a new API key with "Push" permissions

2. **Add the API key to GitHub Secrets:**
   - Go to your repository settings
   - Navigate to **Settings → Secrets and variables → Actions**
   - Click "New repository secret"
   - Name: `NUGET_API_KEY`
   - Value: Your NuGet API key
   - Click "Add secret"

Once configured, the release workflow will automatically publish to NuGet.org.

## Version Control with GitVersion

Versions are automatically calculated based on your Git history and commit messages. Use these commit message tags to control versioning:

- `+semver: major` or `+semver: breaking` → Bumps major version (1.0.0 → 2.0.0)
- `+semver: minor` or `+semver: feature` → Bumps minor version (1.0.0 → 1.1.0)
- `+semver: patch` or `+semver: fix` → Bumps patch version (1.0.0 → 1.0.1)
- `+semver: none` or `+semver: skip` → No version change

**Example:**
```bash
git commit -m "Add new validation feature +semver: minor"
```

## Requirements

These workflows are **free** for public/open-source repositories on GitHub!

For private repositories, GitHub provides free minutes per month:
- Free plan: 2,000 minutes/month
- Pro plan: 3,000 minutes/month

## Troubleshooting

### Tests are failing
- Check the test results artifact in the workflow run
- Review the test logs in the workflow output

### GitVersion is not working
- Ensure `GitVersion.yml` exists in the repository root
- Check that the repository has at least one commit
- Verify the GitVersion configuration is valid

### Release workflow fails to create tag
- Ensure the workflow has write permissions (check repository Settings → Actions → General → Workflow permissions)
- Verify that the tag doesn't already exist

### NuGet publish fails
- Check that `NUGET_API_KEY` secret is correctly set
- Verify the API key has push permissions
- Ensure the package version doesn't already exist on NuGet.org

## Badge

Add this badge to your README.md to show build status:

```markdown
[![Build and Test](https://github.com/mirec75/MJsNetExtensions/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/mirec75/MJsNetExtensions/actions/workflows/build-and-test.yml)
```

Result:
[![Build and Test](https://github.com/mirec75/MJsNetExtensions/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/mirec75/MJsNetExtensions/actions/workflows/build-and-test.yml)
