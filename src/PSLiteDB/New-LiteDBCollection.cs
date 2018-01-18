﻿using LiteDB;
using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace PSLiteDB
{
    [Cmdlet(VerbsCommon.New, "LiteDBCollection")]
    //[CmdletBinding(DefaultParameterSetName = "All")]
    public class NewLiteDBCollection : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0
            )]
        public string Collection { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            Position = 1
            )]
        public LiteDatabase Connection { get; set; }


        protected override void ProcessRecord()
        {
            if (Connection == null)
            {
                try
                {
                    Connection = (LiteDatabase)SessionState.PSVariable.Get("LiteDBPSConnection").Value;
                }
                catch (Exception)
                {

                    throw (new Exception("You must use 'Open-LiteDBConnection' to initiate a connection to a database"));
                }
            }

            var collections = Connection.GetCollectionNames();
            if (Connection.CollectionExists(Collection))
            {
                WriteWarning($"Collection\t['{Collection}'] already exists");
            }
            else
            {
                var col = Connection.GetCollection(Collection);

                //a collection is created during index creation or the first insert so
                //we created a test document, insert it into the collection and then delete the test document
                Dictionary<string, BsonValue> dict = new Dictionary<string, BsonValue>();
                dict.Add("_id", 1);
                dict.Add("test", "test");
                BsonDocument bson = new BsonDocument(dict);
                col.Insert(new BsonDocument(dict));
                col.Delete(1);

            }



        }
    }
}