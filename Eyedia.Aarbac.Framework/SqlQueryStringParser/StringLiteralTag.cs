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
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
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
	#region StringLiteralTag

	[TagType(StringLiteralTag.cTagName)]
	[MatchStringLiteralTag]
	internal class StringLiteralTag : TagBase
	{
		#region Consts

		/// <summary>
		/// The name of the tag (its identifier).
		/// </summary>
		public const string cTagName = "STRING_LITERAL";

		/// <summary>
		/// The tag.
		/// </summary>
		public const string cTagDelimiter = "'";

		#endregion

		#region Methods

		#region Common

		/// <summary>
		/// Reads the tag at the specified position in the specified sql string.
		/// </summary>
		/// <returns>
		/// The position after the tag (at which to continue reading).
		/// </returns>
		protected override int InitializeCoreFromText(SqlStringParserBase parser, string sql, int position, TagBase parentTag)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			int myLiteralStartPos = position;

			position = MatchStartStatic(sql, position);

			if (position < 0)
				throw new Exception("Cannot read the StringLiteral tag.");

			#region Read the literal value

			int myResult = -1;
			int myValueStartPos = position;
			while (position < sql.Length)
			{
				myResult = MatchEnd(sql, position, myLiteralStartPos);
				if (myResult >= 0)
				{
					if (position == myValueStartPos)
						Value = string.Empty;
					else
						Value = sql.Substring(myValueStartPos, position - myValueStartPos);
					break;
				}
				position++;
			}

			if (myResult < 0)
				throw new Exception("Cannot read the StringLiteral tag.");

			#endregion

			Parser = parser;

			HasContents = false;

			return myResult;
		}

		/// <summary>
		/// Returns a value indicating whether there is the tag ending at the specified position.
		/// </summary>
		/// <returns>
		/// If this value is less than zero, then there is no ending; otherwise the 
		/// position after ending is returned.
		/// </returns>
		private int MatchEnd(string sql, int position, int literalStartPos)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			if (string.Compare(sql, position, cTagDelimiter, 0, cTagDelimiter.Length, true) != 0)
				return -1;

			#region Check the next and previous symbols (where there are ' symbols)

			#region Determine the number of ' symbols before

			int myNumberOfApostBefore = 0;
			for (int myCurPos = position - 1; myCurPos >= literalStartPos; myCurPos--)
			{
				if (string.Compare(sql, myCurPos, cTagDelimiter, 0, cTagDelimiter.Length, true) == 0)
					myNumberOfApostBefore++;
				else
					break;
			}

			#endregion

			if (!(myNumberOfApostBefore == 1 && position == literalStartPos + 1))
			{
				if (myNumberOfApostBefore % 2 == 1)
					return -1;

				#region Check whether the next symbol is '

				if (position + 1 < sql.Length)
				{
					if (string.Compare(sql, position + 1, cTagDelimiter, 0, cTagDelimiter.Length, true) == 0)
						return -1;
				}

				#endregion
			}

			#endregion

			return position + cTagDelimiter.Length;
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
			WriteEnd(output);
		}

		/// <summary>
		/// Writes the end of the tag.
		/// </summary>
		private new void WriteEnd(StringBuilder output)
		{
			CheckInitialized();

			#region Check the parameters

			if (output == null)
				throw new ArgumentNullException();

			#endregion

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

	#region MatchStringLiteralTagAttribute

	internal class MatchStringLiteralTagAttribute : MatchTagAttributeBase
	{
		#region Methods

		public override bool Match(string sql, int position)
		{
			return StringLiteralTag.MatchStartStatic(sql, position) >= 0;
		}

		#endregion
	}

	#endregion
}


