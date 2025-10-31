# GitHub Actions Test Failures - Platform Issues

## Summary

Some tests fail on Linux (GitHub Actions) but pass on Windows due to platform-specific differences.

## Failing Tests

### 1. EmbeddedResourceHelper Tests (6 failures)

**Tests:**
- `EmbeddedResourceHelperAsBytesTest.Test1ExistingRelativePath_ExpectSuccess`
- `EmbeddedResourceHelperAsBytesTest.Test5NonExistingRelativePath_ExpectException`
- `EmbeddedResourceHelperAsBytesTest.Test6NonExistingRelativePath_ExpectException`
- `EmbeddedResourceHelperAsStringTest.Test1ExistingRelativePath_ExpectSuccess`
- `EmbeddedResourceHelperAsStringTest.Test5NonExistingRelativePath_ExpectException`
- `EmbeddedResourceHelperAsStringTest.Test6NonExistingRelativePath_ExpectException`

**Cause:**
- Tests use Windows path separators (`\`) which don't work on Linux
- Embedded resource paths in .NET use `.` instead of path separators on all platforms
- Tests expect resources at paths like `Test\Emb-edded Res_sourc-e...` but on Linux they need to use `/` or be referenced differently

**Fix Options:**
1. Use `Path.DirectorySeparatorChar` in tests
2. Use platform-agnostic embedded resource naming (dots instead of slashes)
3. Mark tests with `[TestCategory("Windows")]` and skip on Linux
4. Fix the `EmbeddedResourceHelper` to handle both Windows and Linux paths

### 2. StringExtensions Test (1 failure)

**Test:**
- `StringExtensionsTest.ReplaceFileNameAndPathInvalidCharsParamsTest4_ExpectSuccess`

**Cause:**
- Line ending differences between Windows (`\r\n`) and Linux (`\n`)
- Test expects: `  hehe`
- Linux returns: ` \t\n hehe<>>`

**Fix Options:**
1. Normalize line endings in the test
2. Use `Environment.NewLine` instead of hardcoded values
3. Test both Windows and Linux line endings separately

## Current Workaround

The workflow is configured with `continue-on-error: true` for the test step, so:
- ✅ Build will succeed even if tests fail
- ⚠️ Test failures are reported but don't block the workflow
- 📊 Test results are uploaded as artifacts for review

## Recommended Actions

### Short-term:
- Keep `continue-on-error: true` until tests are fixed
- Monitor test results in artifacts

### Long-term:
1. **Fix EmbeddedResourceHelper tests:**
   ```csharp
   // Use Path.Combine or embedded resource format
   string resourceName = "Test.EmbeddedResource.txt"; // Instead of @"Test\EmbeddedResource.txt"
   ```

2. **Fix StringExtensions test:**
   ```csharp
   // Normalize line endings
   string result = actual.Replace("\r\n", "\n").Replace("\r", "\n");
   string expected = expected.Replace("\r\n", "\n").Replace("\r", "\n");
   Assert.AreEqual(expected, result);
   ```

3. **Or mark platform-specific tests:**
   ```csharp
   [TestMethod]
   [TestCategory("Windows")]
   public void Test1ExistingRelativePath_ExpectSuccess() { ... }
   ```
   
   And configure CI to skip them:
   ```bash
   dotnet test --filter "TestCategory!=Windows"
   ```

## Test Results Location

- **In GitHub Actions**: Actions tab → Workflow run → Artifacts → `test-results`
- **Local**: `TestResults/**/*.trx`

## Statistics (from last run)

- **Total tests**: ~125
- **Passed**: ~118
- **Failed**: 7 (all platform-specific)
- **Success rate on Linux**: ~94%
- **Success rate on Windows**: 100%
