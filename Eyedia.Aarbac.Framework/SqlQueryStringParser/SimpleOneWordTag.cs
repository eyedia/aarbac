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
	#region SimpleOneWordTag

	/// <summary>
	/// The base class for such tags as Select, From etc.
	/// </summary>
	internal abstract class SimpleOneWordTag: TagBase
	{
		#region Fields

		/// <summary>
		/// The value of the ParentTag property.
		/// </summary>
		private TagBase fParentTag;
		
		#endregion

		#region Methods

		#region Common

		/// <summary>
		/// Reads the tag at the specified position in the specified sql.
		/// </summary>
		/// <returns>
		/// The position after the tag (at which to continue reading).
		/// </returns>
		protected override int InitializeCoreFromText(SqlStringParserBase parser, string sql, int position, TagBase parentTag)
		{
			#region Check the arguments

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			int myResult = MatchStart(sql, position);

			if (myResult < 0)
				throw new Exception(string.Format("Cannot read the {0} tag.", Name));

			Parser = parser;

			HasContents = true;

			ParentTag = parentTag;

			return myResult;
		}

		/// <summary>
		/// Checks whether there is the tag at the specified position 
		/// in the specified sql.
		/// </summary>
		/// <returns>
		/// The position after the tag or -1 there is no tag at the position.
		/// </returns>
		protected virtual int MatchStart(string sql, int position)
		{
			return MatchStart(Name, sql, position);
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

			output.Append(Name);
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

			return;
		}

		/// <summary>
		/// Returns a value indicating whether there is the tag ending at the specified position.
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

			if (ParentTag != null && ParentTag.MatchEnd(sql, position) >= 0)
				return position;

			Type myTag = (Parser as SqlStringParser).IsTag(sql, position);
			if (
				myTag != null && 
				!myTag.IsAssignableFrom(typeof(StringLiteralTag)) && 
				!myTag.IsAssignableFrom(typeof(BracesTag))
				)
				return position;

			return -1;
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
		internal static int MatchStart(string name, string sql, int position)
		{
			#region Check the arguments

			if (name == null)
				throw new ArgumentNullException("name");

			SqlStringParserBase.CheckTextAndPositionArguments(sql, position);

			#endregion

			if (string.Compare(sql, position, name, 0, name.Length, true) != 0)
				return -1;

			return position + name.Length;
		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets the name of the tag (its identifier and sql text)
		/// </summary>
		protected abstract string Name { get; }

		/// <summary>
		/// Inidicates whether the string end can be treated as the end of the tag.
		/// </summary>
		public override bool CanTerminateByStringEnd
		{
			get
			{
				CheckInitialized();

				return true;
			}
		}

		/// <summary>
		/// Returns the parent tag of this tag in the sql being parsed.
		/// </summary>
		public TagBase ParentTag
		{
			get
			{
				return fParentTag;
			}
			private set
			{
				fParentTag = value;
			}
		}

		#endregion
	}

	#endregion

	#region MatchSimpleOneWordTagAttribute

	internal abstract class MatchSimpleOneWordTagAttribute: MatchTagAttributeBase
	{
		#region Methods

		public override bool Match(string sql, int position)
		{
			return SimpleOneWordTag.MatchStart(Name, sql, position) >= 0;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the name of the tag (its identifier and sql text)
		/// </summary>
		protected abstract string Name { get; }

		#endregion
	}

	#endregion
}

