namespace XmlValidatorExe
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// small trace wrapper.
	/// </summary>
	internal static class SimpleTracer
	{
		/// <summary>
		/// Just writes a new blank line on the console or line breaks the current line
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prnln")]
		public static void Prnln()
		{
			Console.WriteLine();
		}

		/// <summary>
		/// Prnln does print formated text with args on the standard console out 
		/// and finishes the line with a line break
		/// </summary>
		/// <param name="format">format string</param>
		/// <param name="args">the params</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prnln")]
		public static void Prnln(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}

    public static void PrnString(string message)
    {
      Console.Write(message);
    }

		/// <summary>
		/// Prn does print formated text with args on the standard console out 
		/// and finishes without a line break
		/// </summary>
		/// <param name="format">format string</param>
		/// <param name="args">the params</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prn")]
		public static void Prn(string format, params object[] args)
		{
			Console.Write(format, args);
		}

		/// <summary>
		/// helper to write an exception with an itroducing message on the standard console out 
		/// with a line break at the end
		/// </summary>
		/// <param name="msg">introducing message</param>
		/// <param name="ex">exception beeing traced</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "msg")]
		public static void TraceException(string msg, Exception ex)
		{
            ArgumentException.ThrowIfNullOrWhiteSpace(msg);
            ArgumentNullException.ThrowIfNull(ex);
			Console.WriteLine(msg + " " + ex.ToString());
		}
	}
}
