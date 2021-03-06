﻿//
// Copyright (c) 20015-2017 Jingping Yu.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

namespace Mistral.UniDialogue
{
    /// <summary>
    /// The types of the entries. 
    /// </summary>
    public enum  EntryType
    {
        Conversation,
        Execution,
        Condition,
        Content
    }

    ///Why are all the variables in capital? Because of SQLite4Unity3d. 
    
    /// <summary>
    /// The Base Abstract Class for all DialogueEntries. 
    /// </summary>
	public abstract class DialogueDBEntry
	{
        /// <summary>
        /// The Primary ID Key for the Entry. If set to 0, it means End. 
        /// </summary>
        /// <value>The ID.</value>
		[PrimaryKey]
		public int ID { get; set; }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        /// <returns>The entry type.</returns>
		public abstract EntryType GetEntryType();
		
		public DialogueDBEntry ()
		{
			ID = -1;
		}
	}

	public class ConversationEntry : DialogueDBEntry
	{
        /// <summary>
        /// The name of the Conversation.
        /// </summary>
        public string ConversationName { get; set; }

        /// <summary>
        /// The ID of the first Entry. Could be ExecutionEntry, ConditionEntry or ContentEntry. 
        /// </summary>
        public int FirstEntryID { get; set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConversationEntry"/> class.
		/// </summary>
		/// <param name="cn">Cn.</param>
		/// <param name="nid">Nid.</param>
		public ConversationEntry (string cn, int nid) : base ()
		{
			ConversationName = cn;
			FirstEntryID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConversationEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cn">Cn.</param>
        /// <param name="nid">Nid.</param>
        public ConversationEntry (int id, string cn, int nid)
        {
            ID = id;
            ConversationName = cn;
            FirstEntryID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ConversationEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Conversation;
        }
	}

	public class ExecutionEntry : DialogueDBEntry
	{
        /// <summary>
        /// The YCode to be Executed. 
        /// </summary>
        /// <value>The execution code.</value>
        public string ExecutionCode { get; set; }

        /// <summary>
        /// The ID of the Next Entry. Could be ExecutionEntry, ConditionEntry of ContentEntry. 
        /// </summary>
        /// <value>The next entry I.</value>
        public int NextEntryID { get; set; }

		public ExecutionEntry (string ec, int nid) : base ()
		{
			ExecutionCode = ec;
			NextEntryID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ExecutionEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="ec">Ec.</param>
        /// <param name="nid">Nid.</param>
        public ExecutionEntry (int id, string ec, int nid)
        {
            ID = id;
			ExecutionCode = ec;
            NextEntryID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ExecutionEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Execution;
        }
	}

	public class ConditionEntry : DialogueDBEntry
	{
        /// <summary>
        /// The Condition of Execution this Entry. 
        /// </summary>
        /// <value>The condition code.</value>
        public string ConditionCode { get; set; }

        /// <summary>
        /// The ID of the entry to enter when the condition is met. 
        /// </summary>
        public int SuccessID { get; set; }

        /// <summary>
        /// If the condition is failed, move to the next ConditionEntry. 
        /// </summary>
        public int NextConditionID { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConditionEntry"/> class.
		/// </summary>
		/// <param name="cc">Cc.</param>
		/// <param name="sid">Sid.</param>
		/// <param name="nid">Nid.</param>
		public ConditionEntry (string cc, int sid, int nid) : base ()
		{
			ConditionCode = cc;
			SuccessID = sid;
			NextConditionID = nid;
		}
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Mistral.UniDialogue.ConditionEntry"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="cc">Cc.</param>
        /// <param name="sid">Sid.</param>
        /// <param name="nid">Nid.</param>
        public ConditionEntry (int id, string cc, int sid, int nid)
        {
            ID = id;
            ConditionCode = cc;
            SuccessID = sid;
            NextConditionID = nid;
        }
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ConditionEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Condition;
        }
	}

    public class ContentEntry : DialogueDBEntry
    {
        /// <summary>
        /// The Actors who say the content. In the case of multiple actors, split the actors using '#'. 
        /// </summary>
        public string Actor { get; set; }

        /// <summary>
        /// The content being told. 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The ID of the next Entry. Could be ExecutionEntry, COnditionEntry or ContentEntry. 
        /// </summary>
        public int NextEntryID { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ContentEntry"/> class.
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="c">C.</param>
		/// <param name="nid">Nid.</param>
		public ContentEntry (string a, string c, int nid) : base ()
		{
			Actor = a;
			Content = c;
			NextEntryID = nid;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Mistral.UniDialogue.ContentEntry"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="a">The alpha component.</param>
		/// <param name="c">C.</param>
		/// <param name="nid">Nid.</param>
        public ContentEntry (int id, string a, string c, int nid)
        {
            ID = id;
            Actor = a;
            Content = c;
            NextEntryID = nid;
        }

		/// <summary>
		/// Default Constrcutor
		/// </summary>
		public ContentEntry () : base()
		{
			
		}
		
        public override EntryType GetEntryType ()
        {
            return EntryType.Content;
        }
    }

	/// <summary>
	/// The Administrator of UniDialogue. In charge of creating and deleting database. 
	/// Singleton! 
	/// </summary>
	public static class DialogueDBAdmin
	{
		#region Public Static Variables

		public static string streamingPath
		{
			///DEBUG: Compiler Options are now toggled. With these options exist the MonoDevelop can't function appropriately ... 
			get
			{
//#if UNITY_EDITOR
				return @"Assets/StreamingAssets/";
//#elif UNITY_ANDROID
//				return "jar:file://" + Application.dataPath + "!/assets/";
//#elif UNITY_IOS
//				return Application.dataPath + "/Raw/";
//#else
//				return Application.dataPath + "/StreamingAssets/";
//#endif
			}
		}

		#endregion

		#region Public Static Methods
		
		/// <summary>
		/// Delete the indicated database file. 
		/// </summary>
		/// <returns><c>true</c>, if database was deleted, <c>false</c> otherwise.</returns>
		/// <param name="_dbName">Db name.</param>
		public static bool DeleteDatabase (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				File.Delete(streamingPath + _dbName);
				
				Debug.Log("The Indicated Database has been successfully Removed! ");
				
				return true;
			}
			else
			{
				Debug.Log("Deleting Database Failed! File not Found! ");
				
				return false;
			}
		}
		
		/// <summary>
		/// Creates a new empty database. Do not use the database immediately after this function! 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool CreateDatabase (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				Debug.Log("Unable to Create the Database! A file with the same name has already been created! ");
				
				return false;
			}
			else
			{
				SQLiteConnection _connection = new SQLiteConnection(streamingPath + _dbName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				
				return true;
			}
		}
		
		/// <summary>
		/// Create a new UniDilaougeDatabase. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool CreateUniDialogueDB (string _dbName)
		{
			if (!CreateDatabase(_dbName))
				return false;
			
			InitializeUniDialogueDB(_dbName);
			
			return true;
		}
		
		/// <summary>
		/// Initialize a database into a UniDialogue Database. 
		/// Warning: Won't Drop tables other than the ones used by UniDialogue. And meanwhile all the tables that UniDialogue uses
		/// will be dropped. This operation is not revorable. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		public static bool InitializeUniDialogueDB (string _dbName)
		{
			if (File.Exists(streamingPath + _dbName))
			{
				SQLiteConnection _connection = new SQLiteConnection(streamingPath + _dbName, SQLiteOpenFlags.ReadWrite);
				
				///Drop Old Tables. 
				_connection.DropTable<ConversationEntry>();
				_connection.DropTable<ContentEntry>();
				_connection.DropTable<ConditionEntry>();
				_connection.DropTable<ExecutionEntry>();
				
				///Create New Tables
				_connection.CreateTable<ConversationEntry>();
				_connection.CreateTable<ContentEntry>();
				_connection.CreateTable<ConditionEntry>();
				_connection.CreateTable<ExecutionEntry>();
				
				return true;
			}
			else
			{
				Debug.Log("Unable to Initialized the database! The indicated file is not found! ");
				return false;
			}
		}
		
		/// <summary>
		/// Copy the Database to the indicated path. 
		/// </summary>
		/// <returns><c>true</c>, if up database was backed, <c>false</c> otherwise.</returns>
		/// <param name="_originDB">Origin D.</param>
		/// <param name="_targetPath">Target path.</param>
		public static bool BackUpDatabase (string _originDB, string _targetPath)
		{
			if (!File.Exists(streamingPath + _originDB))
			{
				Debug.Log("The dabatase to backup does not exist. ");
				return false;
			}
			
			if (File.Exists(_targetPath + _originDB))
			{
				File.Delete(_targetPath + _originDB);
			}
			
			File.Copy(streamingPath + _originDB, _targetPath + _originDB);
			
			return true;
		}
		
		#endregion
	}

	/// <summary>
	/// The Dialogue Database Manager. Used Based on a Connection. 
	/// </summary>
	public class DialogueDBManager
	{
		#region Public Variables

		/// <summary>
		/// The Connection to the UniDialogueDB. 
		/// </summary>
		public DialogueDBConnection _connection;

		#endregion
		
		#region Public Attributes
		
		public int NextContentID
		{
			get
			{
				return nextContentID;
			}
			
			private set
			{
				nextContentID = value;
			}
		}
		
		public int NextExecutionID
		{
			get
			{
				return nextExecutionID;
			}
			
			private set
			{
				nextExecutionID = value;
			}
		}
		
		public int NextConditionID
		{
			get
			{
				return nextConditionID;
			}
			
			private set
			{
				nextConditionID = value;
			}
		}
		
		public int NextConversationID
		{
			get
			{
				return nextConversationID;
			}
			
			private set
			{
				nextConversationID = value;
			}
		}
		
		#endregion
		
		#region Private Variables
		
		private int nextContentID;
		
		private int nextExecutionID;
		
		private int nextConditionID;
		
		private int nextConversationID;
		
		#endregion 
		
		#region Constructors
		
		public DialogueDBManager ()
		{
			
		}
		
		public DialogueDBManager (DialogueDBConnection con)
		{
			_connection = con;
			
			///Get the Rows with maximum ID. 
			ConversationEntry _maxConversationEntry = _connection.Query<ConversationEntry>("SELECT *, MAX(ID) FROM ConversationEntry")[0];
			ContentEntry _maxContentEntry = _connection.Query<ContentEntry>("SELECT *, MAX(ID) FROM ContentEntry")[0];
			ExecutionEntry _maxExecutionEntry = _connection.Query<ExecutionEntry>("SELECT *, MAX(ID) FROM ExecutionEntry")[0];
			ConditionEntry _maxConditionEntry = _connection.Query<ConditionEntry>("SELECT *, MAX(ID) FROM ConditionEntry")[0];
			
			///And then set the IDs to the Manager. If a table is empty, then set the start ID. 
			if (_maxConversationEntry.ID != 0)
				NextConversationID = _maxConversationEntry.ID + 1;
			else
				NextConversationID = 1;
			
			if (_maxContentEntry.ID != 0)
				NextContentID = _maxContentEntry.ID + 10;
			else
				NextContentID = 1;
			
			if (_maxExecutionEntry.ID != 0)
				NextExecutionID = _maxExecutionEntry.ID + 10;
			else
				NextExecutionID = 2;
			
			if (_maxConditionEntry.ID != 0)
				NextConditionID = _maxConditionEntry.ID + 10;
			else
				NextConditionID = 3;
		}
		
		#endregion
		
		#region Insert Methods
		
		/// <summary>
		/// Inserts an entry into the database. 
		/// </summary>
		/// <param name="entry">Entry.</param>
		public void InsertEntry (DialogueDBEntry entry)
		{
			EntryType eType = entry.GetEntryType();
			
			int newID;
			
			switch (eType)
			{
				case EntryType.Conversation:
					newID = NextConversationID;
					break;
					
				case EntryType.Condition:
					newID = NextConditionID;
					break;
					
				case EntryType.Content:
					newID = NextContentID;
					break;
					
				case EntryType.Execution:
					newID = NextExecutionID;
					break;
				default:
					newID = 0;
					break;
			}
			
			entry.ID = newID;
			
			try
			{
				_connection.Insert(entry);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			
			switch (eType)
			{
				case EntryType.Conversation:
					NextConversationID++;
					break;
					
				case EntryType.Condition:
					NextConditionID += 10;
					break;
					
				case EntryType.Content:
					NextContentID += 10;
					break;
					
				case EntryType.Execution:
					NextExecutionID += 10;
					break;
				default:
					break;
			}
		}
		
		//These Methods are not welcomed to use ... However in test mode or some situations they could be pretty handy ...
		
		public void InsertConversationEntry (string cname, int startID)
		{
			ConversationEntry toInsert = new ConversationEntry(NextConversationID, cname, startID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextConversationID++;
		}
		
		public void InsertContentEntry (string aname, string cname, int nextID)
		{
			ContentEntry toInsert = new ContentEntry(NextContentID, aname, cname, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextContentID += 10;
		}
		
		public void InsertExecutionEntry (string ycode, int nextID)
		{
			ExecutionEntry toInsert = new ExecutionEntry(NextExecutionID, ycode, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextExecutionID += 10;
		}
		
		public void InsertConditionEntry (string ycode, int sucID, int nextID)
		{
			ConditionEntry toInsert = new ConditionEntry(NextConditionID, ycode, sucID, nextID);
			try
			{
				_connection.Insert(toInsert);
			}
			catch (SQLiteException sex)
			{
				Debug.Log("Entry is not inserted. An error has occured. Check the constraints of the database scheme. "
					+ "The Result is: " + sex.Result
					+ "The Message is: " + sex.Message);
				throw;
			}
			NextConditionID += 10;
		}
		
		//These Methods are not welcomed to use ... However in test mode or some situations they could be pretty handy ...
		
		#endregion
	}

	/// <summary>
	/// The Connection to a UniDialogueDatabase. 
	/// Wraps the SQLiteConnection to make sure that the low-level API is not exposed. 
	/// Always be ready to change the model level. 
	/// </summary>
	public class DialogueDBConnection
	{
		#region Public Variables


		#endregion
		
		#region Private Variables
		
		/// <summary>
		/// The Connection to the SQLite DB. 
		/// </summary>
		private static SQLiteConnection _connection = null;
		
		#endregion
		
		#region Constructors
		
		public DialogueDBConnection ()
		{
			_connection = null;
		}
		
		public DialogueDBConnection (string _dbName, SQLiteOpenFlags authentication)
		{
			EstablishConnection(_dbName, authentication);
		}
		
		#endregion
		
		#region Connection Methods
		
		/// <summary>
		/// Establishes a new connection to the indicated database. 
		/// </summary>
		/// <param name="_dbName">Db name.</param>
		/// <param name="authentication">Authentication.</param>
		public bool EstablishConnection (string _dbName, SQLiteOpenFlags authentication)
		{
			_connection = null;
			
			if (!File.Exists(DialogueDBAdmin.streamingPath + _dbName))
			{
				Debug.Log("Failed to Establish the Connection! The indicated Database does not exist! ");
				return false;
			}
			
			_connection = new SQLiteConnection(DialogueDBAdmin.streamingPath + _dbName, authentication);
			
			Debug.Log("The Connection has been established successfully! ");
			
			return true;
		}
		
		/// <summary>
		/// Disconnects from the current database. 
		/// </summary>
		public void Disconnect ()
		{
			_connection = null;
			
			Debug.Log("Disconnected! ");
		}
		
		#endregion
		
		#region SQL Methods
		
		public List<T> Query<T> (string query, params object[] args) where T : new() 
		{
			return _connection.Query<T> (query, args);
		}
		
		/// <summary>
		/// Safe Insert will never cause an interruption. Mostly used during runtime.
		/// </summary>
		/// <returns>The insert.</returns>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public int SafeInsert (object obj)
		{
			try
			{
				int x = _connection.Insert(obj);
				return x;
			}
			catch (SQLiteException sex)
			{
				Debug.Log("An Error is Occured While Trying to Insert Data. However in Safe Mode the Error is Ignored. "
					+ "The result is: " + sex.Result + "The Message is: " + sex.Message
				);
				return 0;
			}
		}
		
		/// <summary>
		/// The same with SQLite4Unity Insert Function. 
		/// Possibly Trigger an error and stops a MonoBehaviour. 
		/// </summary>
		/// <param name="obj">Object.</param>
		public int Insert (object obj)
		{
			return _connection.Insert(obj);
		}
		
		#endregion
	}
}
