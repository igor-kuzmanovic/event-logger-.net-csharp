﻿using Helpers;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using WCFServiceCommon;

namespace WCFService
{
    internal class WCFService : IWCFService
    {
        public static SecureString PrivateKey { get; set; }

        public byte[] CheckIn()
        {
            byte[] privateKey = new byte[0];

            // Get the client's certificate from the current operation context
            X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(OperationContext.Current);

            // Get the client's name from the certificate
            string clientName = SecurityHelper.GetName(clientCertificate);

            // Log the successful authentication
            EventLogger.AuthenticationSuccess(clientName);

            // Encrypt the private key with the client's certificate public key
            privateKey = RSAEncrypter.Encrypt(StringConverter.ToString(PrivateKey), clientCertificate);

            return privateKey;
        }

        public bool Add(string content)
        {
			bool result = false;
			
            // Get the client's certificate from the current operation context
            X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(OperationContext.Current);

            // Get the client's name from the certificate
            string clientName = SecurityHelper.GetName(clientCertificate);

            // Log the successful authentication
            EventLogger.AuthenticationSuccess(clientName);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Add))
            {
                // If the client lacks the 'Add' permission log the failed authorization attempt
                EventLogger.AuthorizationFailure(clientName, "Add", Permissions.Add.ToString());

                throw new FaultException("Unauthorized");
            }

            // Log the successful authorization
            EventLogger.AuthorizationSuccess(clientName, "Add");

            // Get client's ID represented by the serial number of his certificate
            string userID = clientCertificate.SerialNumber;

            // Execute the 'Add' operation
            result = DatabaseHelper.Add(userID, content);
			
			return result;
        }

        public bool Update(int entryID, string content)
        {
            bool result = false;

            // Get the client's certificate from the current operation context
            X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(OperationContext.Current);

            // Get the client's name from the certificate
            string clientName = SecurityHelper.GetName(clientCertificate);

            // Log the successful authentication
            EventLogger.AuthenticationSuccess(clientName);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Update))
            {
                // If the client lacks the 'Update' permission log the failed authorization attempt
                EventLogger.AuthorizationFailure(clientName, "Update", Permissions.Update.ToString());

                // Record the failed modification attempt
                EventLogger.RecordFailedAttempt(entryID);

                throw new FaultException("Unauthorized");
            }

            // Log the successful authorization
            EventLogger.AuthorizationSuccess(clientName, "Update");

            // Get client's ID represented by the serial number of his certificate
            string userID = clientCertificate.SerialNumber;

            // Execute the 'Update' operation
            result = DatabaseHelper.Update(entryID, userID, content);

            return result;
        }

        public bool Delete(int entryID)
        {
            bool result = false;

            // Get the client's certificate from the current operation context
            X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(OperationContext.Current);

            // Get the client's name from the certificate
            string clientName = SecurityHelper.GetName(clientCertificate);

            // Log the successful authentication
            EventLogger.AuthenticationSuccess(clientName);

            if (!RoleBasedAccessControl.UserHasPermission(clientCertificate, Permissions.Delete))
            {
                // If the client lacks the 'Delete' permission log the failed authorization attempt
                EventLogger.AuthorizationFailure(clientName, "Delete", Permissions.Delete.ToString());

                // Record the failed modification attempt
                EventLogger.RecordFailedAttempt(entryID);

                throw new FaultException("Unauthorized");
            }

            // Log the successful authorization
            EventLogger.AuthorizationSuccess(clientName, "Delete");

            // Execute the 'Delete' operation
            result = DatabaseHelper.Delete(entryID);

            return result;
        }

        public byte[] ReadFile()
        {
            byte[] fileData = new byte[0];

            // Get the client's certificate from the current operation context
            X509Certificate2 clientCertificate = SecurityHelper.GetCertificate(OperationContext.Current);

            // Get the client's name from the certificate
            string clientName = SecurityHelper.GetName(clientCertificate);

            // Log the successful authentication
            EventLogger.AuthenticationSuccess(clientName);

            // Execute the 'ReadFile' operation
            fileData = DatabaseHelper.ReadFile();

            return fileData;
        }
    }
}
