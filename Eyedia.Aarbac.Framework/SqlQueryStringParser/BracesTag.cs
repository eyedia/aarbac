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
	#region BracesTag

	[TagType(BracesTag.cTagName)]
	[MatchBracesTag]
	internal class BracesTag : TagBase
	{
		#region Consts

		/// <summary>
		/// The name of the tag (its identifier).
		/// </summary>
		public const string cTagName = "BRACES";

		/// <summary>
		/// The start of the tag.
		/// </summary>
		public const string cStartTag = "(";

		/// <summary>
		/// The end of the tag.
		/// </summary>
		public const string cEndTag = ")";

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

			int myResult = MatchStartStatic(sql, position);

			if (myResult < 0)
				throw new Exception("Cannot read the Braces tag.");

			Parser = parser;

			HasContents = true;

			return myResult;
		}

		/// <summary>
		/// Returns a value indicating whether there is the tag ending at the specified position.		/// 
		/// </summary>
		/// <returns>
		/// If this value is less than zero, then there is no ending; otherwise the 
		/// position after ending is returned.
		/// </returns>
		public override int MatchEnd(string sql, int position)
		{
			CheckInitialized();

			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			return MatchEndStatic(sql, position);
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

			output.Append(cStartTag);
		}

		/// <summary>
		/// Writes the end of the tag.
		/// </summary>
		public override void WriteEnd(StringBuilder output)
		{
			CheckInitialized();

			#region Check the parameters

			if (output == null)
				throw new ArgumentNullException();

			#endregion

			output.Append(cEndTag);			
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

			if (string.Compare(sql, position, cStartTag, 0, cStartTag.Length, true) != 0)
				return -1;

			return position + cStartTag.Length;
		}

		/// <summary>
		/// Checks whether there is the tag end at the specified position 
		/// in the specified sql.
		/// </summary>
		/// <returns>
		/// The position after the tag or -1 there is no tag end at the position.
		/// </returns>
		public static int MatchEndStatic(string sql, int position)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			if (string.Compare(sql, position, cEndTag, 0, cEndTag.Length, true) != 0)
				return -1;

			return position + cEndTag.Length;
		}

		#endregion

		#endregion		
	}

	#endregion

	#region MatchBracesTagAttribute

	internal class MatchBracesTagAttribute : MatchTagAttributeBase
	{
		#region Methods

		public override bool Match(string sql, int position)
		{
			return BracesTag.MatchStartStatic(sql, position) >= 0;
		}

		#endregion
	}

	#endregion
}

