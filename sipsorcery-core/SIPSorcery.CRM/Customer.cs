﻿// ============================================================================
// FileName: Customer.cs
//
// Description:
// Represents a customer record.
//
// Author(s):
// Aaron Clauson
//
// History:
// 20 May 2009	Aaron Clauson	Created.
//
// License: 
// This software is licensed under the BSD License http://www.opensource.org/licenses/bsd-license.php
//
// Copyright (c) 2009 Aaron Clauson (aaronc@blueface.ie), Blue Face Ltd, Dublin, Ireland (www.blueface.ie)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that 
// the following conditions are met:
//
// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
// disclaimer in the documentation and/or other materials provided with the distribution. Neither the name of Blue Face Ltd. 
// nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
// prior written permission. 
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
// BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SIPSorcery.Sys;
using log4net;

#if !SILVERLIGHT
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
#endif

namespace SIPSorcery.CRM
{
    [Table(Name = "customers")]
    public class Customer : ISIPAsset
    {
        public const string XML_DOCUMENT_ELEMENT_NAME = "customers";
        public const string XML_ELEMENT_NAME = "customer";
        public const string TOPLEVEL_ADMIN_ID = "*";    // If a customer record has their AdminId set to this value they are in charge!

        private static ILog logger = AppState.logger;
        private static string m_newLine = AppState.NewLine;

        [Column(Storage = "_id", Name = "id", DbType = "character varying(36)", IsPrimaryKey = true, CanBeNull = false)]
        public string Id { get; set; }

        [Column(Storage = "_customerusername", Name = "customerusername", DbType = "character varying(32)", CanBeNull = false)]
        public string CustomerUsername { get; set; }

        [Column(Storage = "_customerpassword", Name = "customerpassword", DbType = "character varying(32)", CanBeNull = false)]
        public string CustomerPassword { get; set; }

        [Column(Storage = "_emailaddress", Name = "emailaddress", DbType = "character varying(255)", CanBeNull = false)]
        public string EmailAddress { get; set; }

        [Column(Storage = "_lastname", Name = "lastname", DbType = "character varying(64)")]
        public string LastName { get; set; }

        [Column(Storage = "_firstname", Name = "firstname", DbType = "character varying(64)")]
        public string FirstName { get; set; }

        [Column(Storage = "_city", Name = "city", DbType = "character varying(64)")]
        public string City { get; set; }

        [Column(Storage = "_country", Name = "country", DbType = "character varying(64)")]
        public string Country { get; set; }

        [Column(Storage = "_website", Name = "website", DbType = "character varying(256)")]
        public string WebSite { get; set; }

        [Column(Storage = "_active", Name = "active", DbType = "boolean", CanBeNull = false)]
	    public bool Active{get; set;}

        [Column(Storage = "_suspended", Name = "suspended", DbType = "boolean", CanBeNull = false)]
	    public bool Suspended{get; set;}

        [Column(Storage = "_securityquestion", Name = "securityquestion", DbType = "character varying(256)")]
        public string SecurityQuestion { get; set; }

        [Column(Storage = "_securityanswer", Name = "securityanswer", DbType = "character varying(256)")]
        public string SecurityAnswer { get; set; }

        [Column(Storage = "_createdfromipaddress", Name = "createdfromipaddress", DbType = "character varying(15)")]
        public string CreatedFromIPAddress { get; set; }

        [Column(Storage = "_adminid", Name = "adminid", DbType = "character varying(32)", CanBeNull = true)]
        public string AdminId { get; set; }          // Like a whitelabelid. If set identifies this user as the administrative owner of all accounts that have the same value for their adminmemberid.

        [Column(Storage = "_adminmemberid", Name = "adminmemberid", DbType = "character varying(32)", CanBeNull = true)]
        public string AdminMemberId { get; set; }    // If set it designates this customer as a belonging to the administrative domain of the customer with the same adminid.

        [Column(Storage = "_inserted", Name = "inserted", DbType = "timestamp", CanBeNull = false)]
        public DateTime Inserted { get; set; }

        public object OrderProperty
        {
            get { return CustomerUsername; }
            set { }
        }

        public Customer() { }

#if !SILVERLIGHT

        public Customer(DataRow customerRow) {
            Load(customerRow);
        }

        public void Load(DataRow customerRow) {
            try {
                Id = customerRow["id"] as string;
                CustomerUsername = customerRow["customerusername"] as string;
                CustomerPassword = customerRow["customerpassword"] as string;
                EmailAddress = (customerRow.Table.Columns.Contains("emailaddress") && customerRow["emailaddress"] != null) ?  customerRow["emailaddress"] as string : null;
                AdminId = (customerRow.Table.Columns.Contains("adminid") && customerRow["adminid"] != null) ? customerRow["adminid"] as string : null;
                AdminMemberId = (customerRow.Table.Columns.Contains("adminmemberid") && customerRow["adminmemberid"] != null) ? customerRow["adminmemberid"] as string : null;
                FirstName = (customerRow.Table.Columns.Contains("firstname") && customerRow["firstname"] != null) ? customerRow["firstname"] as string : null;
                LastName = (customerRow.Table.Columns.Contains("lastname") && customerRow["lastname"] != null) ? customerRow["lastname"] as string : null;
                City = (customerRow.Table.Columns.Contains("city") && customerRow["city"] != null) ? customerRow["city"] as string : null;
                Country = (customerRow.Table.Columns.Contains("country") && customerRow["country"] != null) ? customerRow["country"] as string : null;
                SecurityQuestion = (customerRow.Table.Columns.Contains("securityquestion") && customerRow["securityquestion"] != null) ? customerRow["securityquestion"] as string : null;
                SecurityAnswer = (customerRow.Table.Columns.Contains("securityanswer") && customerRow["securityanswer"] != null) ? customerRow["securityanswer"] as string : null;
                WebSite = (customerRow.Table.Columns.Contains("website") && customerRow["website"] != null) ? customerRow["website"] as string : null;
                CreatedFromIPAddress = (customerRow.Table.Columns.Contains("createdfromipaddress") && customerRow["createdfromipaddress"] != null) ? customerRow["createdfromipaddress"] as string : null;
                Inserted = (customerRow.Table.Columns.Contains("inserted") && customerRow["inserted"] != null) ? Convert.ToDateTime(customerRow["inserted"]) : DateTime.MinValue;
            }
            catch (Exception excp) {
                logger.Error("Exception Customer Load. " + excp.Message);
                throw;
            }
        }

        public Dictionary<Guid, object> Load(XmlDocument dom) {
            return SIPAssetXMLPersistor<Customer>.LoadAssetsFromXMLRecordSet(dom);
        }

#endif

        public object GetOrderProperty()
        {
            return CustomerUsername;
        }

        public string ToXML()
        {
            string customerXML =
                " <" + XML_ELEMENT_NAME + ">" + m_newLine +
               ToXMLNoParent() + m_newLine +
                " </" + XML_ELEMENT_NAME + ">" + m_newLine;

            return customerXML;
        }

         public string ToXMLNoParent() {
             string customerXML =
                 "  <id>" + Id + "</id>" + m_newLine +
                 "  <customerusername>" + CustomerUsername + "</customerusername>" + m_newLine +
                 "  <customerpassword>" + CustomerPassword + "</customerpassword>" + m_newLine +
                 "  <emailaddress>" + EmailAddress + "</emailaddress>" + m_newLine +
                 "  <firstname>" + SafeXML.MakeSafeXML(FirstName) + "</firstname>" + m_newLine +
                 "  <lastname>" + SafeXML.MakeSafeXML(LastName) + "</lastname>" + m_newLine +
                 "  <city>" + SafeXML.MakeSafeXML(City) + "</city>" + m_newLine +
                 "  <country>" + Country + "</country>" + m_newLine +
                 "  <adminid>" + AdminId + "</adminid>" + m_newLine +
                 "  <adminmemberid>" + AdminMemberId + "</adminmemberid>" + m_newLine +
                 "  <website>" + SafeXML.MakeSafeXML(WebSite) + "</website>" + m_newLine +
                 "  <securityquestion>" + SecurityQuestion + "</securityquestion>" + m_newLine +
                 "  <securityanswer>" + SafeXML.MakeSafeXML(SecurityAnswer) + "</securityanswer>" + m_newLine +
                 "  <createdfromipaddress>" + CreatedFromIPAddress + "</createdfromipaddress>" + m_newLine +
                 "  <inserted>" + Inserted.ToString("dd MMM yyyy HH:mm:ss") + "</inserted>" + m_newLine +
                 "  <active>" + Active + "</active>" + m_newLine +
                 "  <suspended>" + Suspended + "</suspended>";


             return customerXML;
         }

        public string GetXMLElementName() {
            return XML_ELEMENT_NAME;
        }

        public string GetXMLDocumentElementName() {
            return XML_DOCUMENT_ELEMENT_NAME;
        }
    }
}
