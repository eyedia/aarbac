#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Synechron Holdings Inc BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#region Developer Information
/*
Author  - Debjyoti Das (debjyoti@debjyoti.com)
Created - 11/12/2017 3:31:31 PM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice
using System;
using System.Collections.Generic;
using System.Text;

namespace Eyedia.Aarbac.Framework.SqlQueryStringParser
{
	#region QuotedIdentifierTag

	[TagType(QuotedIdentifierTag.cTagName)]
	[MatchQuotedIdentifierTag]
	internal class QuotedIdentifierTag : TagBase
	{
		#region Consts

		/// <summary>
		/// The name of the tag (its identifier).
		/// </summary>
		public const string cTagName = "QUOTED_IDENTIFIER";

		/// <summary>
		/// The tag delimiter.
		/// </summary>
		public const string cTagDelimiter = "\"";

		#endregion

		#region Methods

		#region Common

		/// <summary>
		/// Reads the tag at the specified position in the specified word and separator array.
		/// </summary>
		/// <returns>
		/// The position after the tag (at which to continue reading).
		/// </returns>
		protected override int InitializeCoreFromText(SqlStringParserBase parser, string sql, int position, TagBase parentTag)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			int myAfterTagStartPos = MatchStartStatic(sql, position);

			if (myAfterTagStartPos < 0)
				throw new Exception("Cannot read the QuotedIdentifier tag.");

			#region Read the identifier's value

			int myTagEndPos = sql.IndexOf(cTagDelimiter, myAfterTagStartPos, StringComparison.InvariantCultureIgnoreCase);
			if (myTagEndPos < 0)
				throw new Exception("Cannot read the QuotedIdentifier tag.");

			if (myAfterTagStartPos == myTagEndPos)
				Value = string.Empty;
			else
				Value = sql.Substring(myAfterTagStartPos, myTagEndPos - myAfterTagStartPos);

			#endregion

			Parser = parser;

			HasContents = false;

			return myTagEndPos + cTagDelimiter.Length;
		}

		/// <summary>
		/// Writes the start of the tag.
		/// </summary>
		public override void WriteStart(StringBuilder output)
		{
			CheckInitialized();

			#region Check the parameters

			if (output == null)
				throw new ArgumentNullException();

			#endregion

			output.Append(cTagDelimiter);
			output.Append(Value);
			output.Append(cTagDelimiter);
		}

		#endregion

		#region Static

		/// <summary>
		/// Checks whether there is the tag start at the specified position 
		/// in the specified sql.
		/// </summary>
		/// <returns>
		/// The position after the tag or -1 there is no tag start at the position.
		/// </returns>
		public static int MatchStartStatic(string sql, int position)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			if (string.Compare(sql, position, cTagDelimiter, 0, cTagDelimiter.Length, true) != 0)
				return -1;

			return position + cTagDelimiter.Length;
		}

		#endregion

		#endregion		
	}

	#endregion

	#region MatchQuotedIdentifierTagAttribute

	internal class MatchQuotedIdentifierTagAttribute : MatchTagAttributeBase
	{
		#region Methods

		public override bool Match(string sql, int position)
		{
			return QuotedIdentifierTag.MatchStartStatic(sql, position) >= 0;
		}

		#endregion
	}

	#endregion
}

