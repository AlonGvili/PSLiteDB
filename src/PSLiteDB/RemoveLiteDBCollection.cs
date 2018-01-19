﻿using LiteDB;
using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace PSLiteDB
{
    [Cmdlet(VerbsCommon.Remove, "LiteDBCollection",ConfirmImpact = ConfirmImpact.High)]
    public class RemoveLiteDBCollection : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = false,
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


            if (!Connection.CollectionExists(Collection))
            {
                WriteWarning($"Collection\t['{Collection}'] does not exist");
            }
            else
            {

                if (ShouldProcess(Collection,"Delete Collection with all data & indexes"))
                {
                    try
                    {
                        Connection.DropCollection(Collection);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                        
                }
                   
            }
        }
    }
}