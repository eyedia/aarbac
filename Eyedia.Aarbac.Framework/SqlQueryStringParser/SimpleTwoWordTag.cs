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
	#region SimpleTwoWordTag

	/// <summary>
	/// The base class for such tags as Order By, Group By etc.
	/// </summary>
	internal abstract class SimpleTwoWordTag : SimpleOneWordTag
	{
		#region Methods

		#region Common

		/// <summary>
		/// Checks whether there is the tag at the specified position 
		/// in the specified sql.
		/// </summary>
		/// <returns>
		/// The position after the tag or -1 there is no tag at the position.
		/// </returns>
		protected override int MatchStart(string sql, int position)
		{
			return MatchStart(FirstWord, SecondWord, sql, position);
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

			output.Append(FirstWord);
			output.Append(SqlStringParserBase.cWhiteSpace);
			output.Append(SecondWord);
		}

		#endregion

		#region Static

		/// <summary>
		/// Checks whether there is the tag at the specified position 
		/// in the specified sql.
		/// </summary>
		/// <name>The value of the Name property.</name>
		/// <returns>
		/// The position after the tag or -1 there is no tag at the position.
		/// </returns>
		internal static int MatchStart(string firstWord, string secondWord, string sql, int position)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			if (string.Compare(sql, position, firstWord, 0, firstWord.Length, true) != 0)
				return -1;

			position += firstWord.Length;

			SqlStringParserBase.SkipWhiteSpace(sql, ref position);
			if (position == sql.Length)
				return -1;

			if (string.Compare(sql, position, secondWord, 0, secondWord.Length, true) != 0)
				return -1;

			return position + secondWord.Length;
		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets the first word of the tag.
		/// </summary>
		protected abstract string FirstWord { get; }

		/// <summary>
		/// Gets the second word of the tag.
		/// </summary>
		protected abstract string SecondWord { get; }

		#endregion
	}

	#endregion

	#region MatchSimpleTwoWordTagAttribute

	internal abstract class MatchSimpleTwoWordTagAttribute : MatchSimpleOneWordTagAttribute
	{
		#region Methods

		public override bool Match(string sql, int position)
		{
			return SimpleTwoWordTag.MatchStart(FirstWord, SecondWord, sql, position) >= 0;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the first word of the tag.
		/// </summary>
		protected abstract string FirstWord { get; }

		/// <summary>
		/// Gets the second word of the tag.
		/// </summary>
		protected abstract string SecondWord { get; }

		#endregion
	}

	#endregion
}


