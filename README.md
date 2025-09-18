# Table of Contents
[TOC]

# About MJsNetExtensions

MJsNetExtensions represents a collection of common .Net utilities, helpers and extensions, which shall make the everyday coding easier.

You can find the newest NuGet package here: ...

The MJsNetExtensions sources are  [here](MJsNetExtensions)

In the next chapters follows the information about the contained features:


# Simplified method parameter checking - Throw.IfNull() & co.
This + Strongly-Typed Object Validation + Operation Result Pattern are my favorite extensions and the 1st point why I started to write this extensions.

It serves to reduce the nasty parameter checking lines to a reasonable 1 line per parameter minimum, thus reducing your code and making it more readable. Especially, if you turn on “Code Analysis” on each build, the compiler hints you about using a method parameter without checking first, whether if it is not null etc.(it is really useful and recommended to turn CA on build on!).

It reduces (replaces) code like this 8 lines (4 lines per parameter):
```C#
if (context == null)
{
    throw new ArgumentNullException(nameof(context));
}
if (string.IsNullOrWhiteSpace(elementName))
{
    throw new ArgumentNullException(nameof(elementName));
}
IEnumerable<int> portNumbers = ...;
if (portNumbers == null || !portNumbers.Any())
{
    throw new ArgumentNullException(nameof(elementName));
}
```

into single compact line per parameter check like this:
```C#
Throw.IfNull(context, nameof(context));
Throw.IfNullOrWhiteSpace(elementName, nameof(elementName));

IEnumerable<int> portNumbers = ...;
Throw.IfNullOrEmpty(portNumbers, nameof(portNumbers));
```

Or even like this extension methods:
```C#
this.Context     = context    .ThrowIfNull            (nameof(context));
this.ElementName = elementName.ThrowIfNullOrWhiteSpace(nameof(elementName));
this.PortNumbers = portNumbers.ThrowIfNullOrEmpty     (nameof(portNumbers));
```

# Strongly-Typed Object Validation: `ISimpleValidatable` and `IValidatable`, or `ISimpleValidatableAndUpdatable` and `IValidatableAndUpdatable`
This emerging design pattern uses `ValidationResult` object to fix an old problem of using methods like: `bool IsValid(out string invalidReason) {...}` and leveraging the Strongly-Typed Object Validation to use validation result object instead.

This problem and solution of this emerging design pattern (as of April 2021) is roughly described here:
+ https://keestalkstech.com/2019/09/validate-strongly-typed-options-when-using-config-sections/
  Same Author and Topic: https://medium.com/wehkamp-techblog/validate-strongly-typed-options-when-using-config-sections-fffbadd30821
+ https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-asp-net-core/
+ https://www.simongilbert.net/validation-rules-fluentvalidation-csharp-dotnetcore/

There are some interesting approaches out there:
+ https://fluentvalidation.net/
+ http://breeze.github.io/doc-cs/validation.html

...but I needed something, where the class hierarchy, defined e.g. by an XML Schema, after it is being XML validated and parsed into a C# object, could be automatically hierarchically validated.

My approach was to put the validation logic inside the data classes, instead of defining extra parallel validation classes, like in a case of FluentValidation or Breeze. The class implementor shall not have to deal manually with class hierarchy validation propagation, but concentrate just on own "simple" properties, i.e. those, not implementing any of the `I*Validatable*` interfaces.

There are many examples of validation based on `I*Validatable*` interfaces. One simple real life example contained in MJsNetExtensions is SMTP email sending. This example follows:

You can now simplify bulky code like this OLD one:
```C#
// Validatable class:
public class SmtpClientSettings
{
  ...
  public bool IsValid(out string invalidReason)
  {
      bool isValid = true;
      List<string> invalidReasons = new List<string>();
  
      if (string.IsNullOrWhiteSpace(this.SmtpHost))
      {
          invalidReasons.Add($"nameof(this.SmtpHost) shall not be empty");
          isValid = false;
      }
  
      if (this.Port < 1)
      {
          invalidReasons.Add(string.Format(CultureInfo.InvariantCulture, "{0} is invalid: {1}", nameof(this.Port), this.Port));
          isValid = false;
      }
      ...
  
      invalidReason =  isValid?  null  :  string.Join(", ", invalidReasons);
      return isValid;
  }
}

// Caller
public void SendEmail(SmtpClientSettings settings)
{
  Throw.IfNull(settings, nameof(settings));

  string invalidReason;
  bool ret = IsValid(out invalidReason);
  if (!ret)
  {
      Logger.Error(invalidReason);
  }

  Throw.InvalidOperationIfFalse(ret, invalidReason);
  ...
}
```

Into a simple and sleek code like this NEW one:
```C#
// Validatable class:
public class SmtpClientSettings : ISimpleValidatable
{
    public string SmtpHost { get; set; } = null;

    public int Port { get; set; } = 25;

    public bool EnableSsl { get; set; } = false;

    public ICredentialsByHost Credentials { get; set; } = (ICredentialsByHost)CredentialCache.DefaultCredentials;

    public void PreStructureValidation([ValidatedNotNull] ValidationResult validationResult)
    {
        validationResult
            .ThrowIfNull(nameof(validationResult))
            .InvalidateIfNullOrWhiteSpace(this.SmtpHost, nameof(this.SmtpHost));

        validationResult.InvalidateIf(this.Port < 1, nameof(this.Port), "must be > 0, but is: {0}", this.Port);

        validationResult.InvalidateIfNull(this.Credentials, nameof(this.Credentials));
    }
}

// Caller
public void SendEmail(SmtpClientSettings settings)
{
  // following line does check 1st if the settings is not null and then validates the ISimpleValidatable
  Throw.IfNullOrInvalid(settings, nameof(settings));
  ...
}
```

## Strongly-Typed Object Validation Examples
E.g. all projects based on:
base.xmlconfigbase
And there are at least 4 of them as of April 2021.

SWIMP / MSV3 XML request and response validation is forseen to use this technology.

# The `OperationResult` Pattern
The problem and solution of this emerging design pattern (as of April 2021) is roughly described here:

+ (2018) The Operation Result Pattern — A Simple Guide
https://medium.com/@cummingsi1993/the-operation-result-pattern-a-simple-guide-fe10ff959080#
+ (2015) Error Handling in SOLID C# .NET – The Operation Result
https://www.codeproject.com/Articles/1022462/Error-Handling-in-SOLID-Csharp-NET-The-Operation-R
+ Is there an alternative to the Notification pattern for multiple messages and success/failure?
https://stackoverflow.com/questions/40404327/is-there-an-alternative-to-the-notification-pattern-for-multiple-messages-and-su
+ DotNET: Why catch(Exception)/empty catch is bad
https://devblogs.microsoft.com/dotnet/why-catchexceptionempty-catch-is-bad/#:~:text=Empty%20catch%20statements%20can%20be,even%20non%2DCLS%20compliant%20exceptions.&text=If%20you%20use%20a%20StreamReader,and%20handle%E2%80%94these%20two%20exceptions.

In MJsNetExtensions there are 2 classes `OperationResult` `OperationResult<T>` implementing this pattern and many usages already in the MJsNetExtensions solution.
See e.g.: `SmtpMailSender.SendMail(...)` implementation.

# IO Helpers

There are IO helpers to:
+ simplify reading a file into a `MemoryStream` and for writing `MemoryStream` back to a file. This is useful if processing the data in several steps, to avoid the slow IO operations and process the data "in memory".
+ compute MD5 Checksum of a file or a stream. This is useful, e.g. in Unit Testing if comparing file output with the wished ideal output file
+ directory creation “cache” of already created directories, to save performance via reducing file system check or create operations

# XML – XSD / DTD Validation – made properly
Note that the XML validation provided in .Net has following 2 lacks:

1. if you validate an XML to an XSD, and the XML does not correspond to the XSD at all (e.g. validating machine engineering XML to a bookstore XSD), then the .Net validating says “All OK” – the XML is valid! This is usually pretty wrong in comparison to what you would like to do, or expect.
    This lack results in bugs, like if you e.g. misspell e.g. the XML namespace or XML root element name, then this .Net validation “feature” says: “All right, the XML is valid!” – which is in reality pretty wrong!
2. Performance drawback because of resolving XSDs on the public (company / institute / etc.) web pages, each time you validate a piece of XML containing XSD location(s).


The `XmlXsdValidator` and `XmlDtdValidator` contained in the MJsNetExtensions solve both of this issues properly. The first solved point is an implementation detail. You can have a look at the code if needed. The second:

## Solved performance issues

The `XmlValidator` is its own XML Validation Facade.
`XmlValidator.Create(XmlValidatorSettings settings)` is a Generic Factory method for creating and initializing of an XSD or DTD XML Validator.

An example:

```C#
// Prepare: select XSDs in settings:
IEnumerable<string> xsdFilePaths = ...;
XmlValidatorSettings settings = new XmlValidatorSettings
{
    //XmlValidationType = XmlValidationType.XSD,     // <--> XSD is default. This can be overriden to do an old DTD validation. 

    // Optional. Convenient to use, if there is a single XSD file path, or URI, or UNC.
    XmlDefinitionFilePath = mainXsdFilePath,

    // Optional. Convenient to use, if there is aremore than 1 XSD file paths, and/or URIs, and/or UNC.
    AdditionalXmlDefinitionFilePaths = xsdFilePaths,
};

// Do XML validation:
// This can be called many times for a single instance "myValidator".
var myValidator = XmlValidator.Create(settings);

// Process validation result:
XmlValidationResult result = this.Validator.ValidateOneXml(xmlFile);
if (!result.IsValid)
{
    Logger.Error(result); // the overall message with count of errors and warnings will be output, and all single error and warning follows as a next line
}
```

In this way, you explicitly create an `XmlValidator`, which reads and resolves the XSD(s) just once and possibly from some local directory and NOT from Web! And then uses this read and parsed XSD model(s) to validate the XML contents. You can validate many XML files with a single `XmlValidator` instance. Consider, that:

+ reading a file from local directory may be 10 to 100 times faster, then accessing a remote XSD location URL!
+ you need to create and thus parse the XSDs into .Net memory representation just once, to validate many XMLs, thus saving the XSD parsing every time

**NOTE:** that you can provide several heterogeneous XSDs to a single `XmlXsdValidator` or DTDs to an `XmlDtdValidator` and thus allowing it to validate different kinds of XMLs. E.g. providing XSDs for Delivery Notes and Order Requests to the `XmlValidator` enables you to validate delivery notes XMLs as well as order request XMLs if needed with a single `XmlValidator` instance.

Another XML validation examples:

```C#
// xmlFileName is theoretical or a real path. IT WILL NOT BE ACCESSED! It servees merely as a name for validation messages.
string xmlFileName = "fooBar.xml";

// Do single XML contained in a string validation here. 
// Remember, you can call the validation many times for different XMLs with this single 'xmlValidator' instance. 
// You have following 4 extra possibilities next to a XML file validation:
result = myValidator.ValidateOneXml(xmlString, xmlFileName);
 
// or:
XmlReader inputReader = ...;
result = myValidator.ValidateOneXml(inputReader, xmlFileName); 
 
// or:
TextReader inputReader = ...;
result = myValidator.ValidateOneXml(inputReader, xmlFileName);
 
// or:
Stream inputStream = ...;
result = myValidator.ValidateOneXml(inputStream, xmlFileName);
```


# XML Deserialization = XML Validation *AND* Strongly-Typed Object Validation Combined!
See the `XmlDeserializationExtensions` with `XmlDeserializationSettings<T>` in the UnitTests for a simultaneous single step XML and Strongly-Typed Object Validation!

The 1st main usage is: XmlConfigBase and all projects based on it. See:
base.xmlconfigbase
And there are at least 4 of them as of April 2021.

SWIMP / MSV3 XML request and response validation is foreseen to use this technology.
SWIMP-CH already uses this optimized technology for request and response validation and deserialization.

XML Deserialization provides a family of extension methods `XmlTo()` and `XmlToAndValidate()` (and derivates) to parse XML string, or load XML file, or read from XmlReader, TextReader or Stream.


# XML Serialization - supporting Default Namespace, xsi:schemaLocation and xsi:noNamespaceSchemaLocation

There is a list of `ToXml()` methods simplifying serialization to XML string, file, XmlWriter, TextWriter and Stream.
It was developed originally to simplify SWIMP response serializing purposes. This approach simplifies previous several XML serialization steps into one generic step writing all the necessary Default Namespace, xsi:schemaLocation or xsi:noNamespaceSchemaLocation into the resulting XML.

# General extensions
+ `Trim()` – `DateTime` to a specific precision, e.g. to a whole second, or hour, or day, etc.
+ `TruncateToMilliseconds()` – truncates `TimeSpan` to milliseconds
+ Generalized `Max()` and `Min()` to not only `Math.Max()` and `Min()`

String or StringBuilder extensions:
+ `TrimLeadingZeros()`
+ `Contains()` – using StringComparison for e.g.  a case insensitive comparison
+ `EscapeToCsvCell()` – escape a string to insert it without conflicts into a CSV as a value
+ `EscapeForFurtherFormatting()` – Escape string for further formatting, i.e. it doubles each `{` to `{{` and `}` to `}}`.
+ `ReplaceStrings()` – uses a dictionary and for each key/value pair of it replaces all occurrences of the  key with the value. You can decide about case sensitivity in the searching for “keys”.
+ `TryFormat()` and `TryFormatInvariant()` – a fail safe `string.Format(...)` variants, which do not throw, but return an error message decorated format string and params, simmilar like Log4net does on formatting errors. This is used for optional formatting message and arguments, e.g. `Throw.IfTrue(bool conditionResult, string name, string messageFormat, params object[] args)`
+ `SplitHumanReadableTextToLines()` – splits text to lines according to given line length. Useful e.g. for filling PostScript invoices, advices, etc.
+ `RemoveDiacritics()` – replaces all accented letters through their not accented counterparts, e.g. `é` to `e`, or `ä` to `ae`. Useful e.g. for filling PostScript invoices, advices, etc.
+ `StringBuilder.AppendFormatLine()` – it just combines `sb.AppendFormat()` and `sb.Append('\n');` and makes the CA-requested `CultureInfo.InvariantCulture` unnecessary in the call, because it is used inside.

LINQ:
+ `Interleave()`
+ `IndexOf()` – but it makes sense only if the enumerable keeps its elements in a sequence

# Simple SMTP Mail Sending 
See `SmtpMailSender`, with `SmtpMailMessage` and `SmtpClientSettings` for more.

The very first real life usage can be found here:
salesweb.swimp
dev\src\SWIMP-CH\Swimp-CH.sln

# TPL Task extensions
TPL Task Extensions are useful, if following scenarios:
+ `IgnoreExceptionsOnNoWaitedTask()` – this extension method “swallows” a task’s unhandled exceptions:
```C#
Task.Factory.StartNew(
  ...,
  cancellationToken,
  TaskCreationOptions.LongRunning,
  TaskScheduler.Default
).IgnoreExceptionsOnNoWaitedTask();
```

You can read more on this here:
 Joe Albahari - PART 5: PARALLEL PROGRAMMING
 <http://www.albahari.com/threading/part5.aspx>

+ `WithThrowOnCancellation()` – method which enables to cancel blocking `await`-s and terminate them with TaskCanceledException. Use it e.g. like this:
```C#
string request = await reader.ReadLineAsync()
                       .WithThrowOnCancellation(cancellationToken);
```

+ `WithSilentFinishOnCancellation()` – method which enables to cancel blocking `await`-s “silently” without propagating TaskCanceledException. However, the use is not recommended, due to requiring user to write handling code after each await, which shell then evaluate if the `await` finished due to cancelation or not.

# Windows RunAs Impersonation: Interactive LogOn "RunAs"
If you need to impersonate to an another account to run specific code, then you do it like this:
WinRunAsImpersonation.InteractiveLogOnRunAs(
```C#
    "CUSTOMER_DOMAIN", "svc.salesquery", "thePwdAsPlainText", 
    () => DoFullTourDepartureAnalysisInner(Denu05ms0053MSSQLconnectionString),
    Logger.Info, Logger.Error
    );
```

To use this functionality I recommend to log on once with the user (e.g. the "CUSTOMER_DOMAIN\svc.salesquery") on the computer, where you want to run the code.
Then all the Registry data and Windows User files and directories, like %AppData% are available on the system.

Read more on this:
1. [MSDN: WindowsIdentity.Impersonate Method](https://docs.microsoft.com/en-us/dotnet/api/system.security.principal.windowsidentity.impersonate?redirectedfrom=MSDN&view=netframework-4.8#System_Security_Principal_WindowsIdentity_Impersonate)
2. [MSDN: Windows Authentication to make SQL Server Connection](https://forums.asp.net/t/2126438.aspx?Windows+Authentication+to+make+SQL+Server+Connection)
3. [StackOverflow: LogonUser, LOGON32_LOGON_INTERACTIVE and LOGON32_LOGON_NETWORK](https://stackoverflow.com/questions/1501704/logonuser-logon32-logon-interactive-and-logon32-logon-network)
4. [Network vs. interactive logon](https://www.bitvise.com/wug-logontype)

Contributing to MJsNetExtensions
======================

You have found a bug or you have an idea for a cool new feature?

Contributing code is a great way to give something back to the developer community. Before you dig right into the code there are a few guidelines that we need contributors to follow so that we can have a chance of keeping on top of things.

Making Changes
--------------
+ Create a topic branch from where you want to base your work (this is the `develop` branch).
+ Make commits of logical units.
+ Respect the original code style:
  + Create minimal diffs - disable on save actions like reformat source code or organize imports. If you feel the source code should be reformatted discuss with the initiators of this repository.
+ Make sure your commit messages are in the proper format.
+ Make sure you have added the necessary tests for your changes.
+ Run all the unit tests to assure nothing else was accidentally broken.
+ Document your changes in this ReadMe where everyone can get a clue of the new feature.
+ After finishing, consult your changes shortly with the initiators of this repository and plan merge into `develop` with them.
+ Update the version number in `...\MJsNetExtensions\dev\src\MJsNetExtensions\Properties\AssemblyInfo.cs` after merge into `develop`.
+ Organize when to do `TFS build` and `release` with the initiators of this repository.

Making Trivial Changes
----------------------

For changes of a trivial nature to comments and documentation, it is not always necessary to create a new branch. In this case, it is appropriate to do the change also in `develop` branch.

