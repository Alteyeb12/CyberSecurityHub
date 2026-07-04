# Contributing to CyberInsight

## Contributing Guidelines

Thank you for your interest in contributing to CyberInsight! We welcome all contributions.

### How to Contribute

#### 1. Fork the Repository
```bash
git clone https://github.com/your-username/CyberSecurityHub.git
cd CyberSecurityHub
```

#### 2. Create a New Branch
```bash
git checkout -b feature/your-feature-name
```

#### 3. Make Changes
- Follow existing code standards
- Add comments for complex code
- Test changes locally

#### 4. Commit Changes
```bash
git add .
git commit -m "Add: description of changes"
```

#### 5. Push to Branch
```bash
git push origin feature/your-feature-name
```

#### 6. Open Pull Request
- Go to GitHub
- Click "New Pull Request"
- Select your branch
- Complete PR description
- Wait for review

---

## Code Standards

### C# Coding Standards
```csharp
// ✅ Use PascalCase for classes and methods
public class UserService
{
    // ✅ Use camelCase for local variables
    public async Task<User> GetUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user;
    }
}

// ❌ Don't use unclear naming
public class usr { } // Error

// ❌ Don't leave commented code
// var x = y; // Old code
```

### Expected Practices
```csharp
// ✅ Add exception handling
try
{
    var result = await service.DoSomethingAsync();
}
catch (Exception ex)
{
    _logger.LogError($"Error: {ex.Message}");
    throw;
}

// ✅ Use async/await
public async Task<Result> MyMethodAsync()
{
    return await _repository.GetDataAsync();
}

// ❌ Avoid synchronous code when possible
public Result MyMethod() // Avoid
{
    return _repository.GetData().Result; // Avoid
}
```

---

## Welcomed Contribution Types

### ✅ New Features
- Add new features
- UI improvements
- Performance improvements

### ✅ Bug Fixes
- Fix known bugs
- Security improvements
- Reliability improvements

### ✅ Documentation
- Documentation improvements
- Add examples
- Translations

### ✅ Tests
- Add new tests
- Improve test coverage
- Security tests

---

## Review Process

### PR will be reviewed based on:

1. **Quality**
   - Code standards compliance
   - No obvious errors
   - Good performance

2. **Tests**
   - Sufficient tests
   - All tests pass
   - Adequate coverage

3. **Documentation**
   - Clear documentation
   - Helpful comments
   - Clear commit message

4. **Compatibility**
   - No breaking changes
   - Version compatibility
   - No conflicts with other PRs

---

## Feedback and Comments

### During review we may ask for:
- Code improvements
- Add tests
- Documentation improvements
- Bug fixes

### Your response to feedback:
- Make requested changes
- Explain if there's misunderstanding
- Ask for clarification if needed

---

## Pre-submission Checklist

- [ ] Followed code standards
- [ ] Tested changes locally
- [ ] Didn't break existing tests
- [ ] Added tests for new code
- [ ] Documented changes
- [ ] Wrote clear commit message
- [ ] No unwanted files
- [ ] Followed contribution rules

---

## FAQ

**Q: How long does PR review take?**
A: Usually 1-3 business days

**Q: Can I ask questions before opening a PR?**
A: Yes, open an Issue first

**Q: What if my PR is rejected?**
A: We'll explain why and you can make improvements

**Q: Can I contribute in another language?**
A: Yes, we welcome translations

---

## Support

For questions and inquiries:
- 📧 Email: support@cyberinsight.com
- 💬 Forum: GitHub Discussions
- 🐛 Bugs: GitHub Issues

---

**Thank you for contributing!** 🎉
