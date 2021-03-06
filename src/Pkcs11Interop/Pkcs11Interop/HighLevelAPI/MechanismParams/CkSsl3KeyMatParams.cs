/*
 *  Copyright 2012-2016 The Pkcs11Interop Project
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

/*
 *  Written for the Pkcs11Interop project by:
 *  Jaroslav IMRICH <jimrich@jimrich.sk>
 */

using System;
using Net.Pkcs11Interop.Common;

namespace Net.Pkcs11Interop.HighLevelAPI.MechanismParams
{
    /// <summary>
    /// Parameters for the CKM_SSL3_KEY_AND_MAC_DERIVE mechanism
    /// </summary>
    public class CkSsl3KeyMatParams : IMechanismParams, IDisposable
    {
        /// <summary>
        /// Flag indicating whether instance has been disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Platform specific CkSsl3KeyMatParams
        /// </summary>
        private HighLevelAPI40.MechanismParams.CkSsl3KeyMatParams _params40 = null;

        /// <summary>
        /// Platform specific CkSsl3KeyMatParams
        /// </summary>
        private HighLevelAPI41.MechanismParams.CkSsl3KeyMatParams _params41 = null;

        /// <summary>
        /// Platform specific CkSsl3KeyMatParams
        /// </summary>
        private HighLevelAPI80.MechanismParams.CkSsl3KeyMatParams _params80 = null;

        /// <summary>
        /// Platform specific CkSsl3KeyMatParams
        /// </summary>
        private HighLevelAPI81.MechanismParams.CkSsl3KeyMatParams _params81 = null;

        /// <summary>
        /// Flag indicating whether object with returned key material has left this instance
        /// </summary>
        private bool _returnedKeyMaterialLeftInstance = false;

        /// <summary>
        /// Resulting key handles and initialization vectors after performing a DeriveKey method
        /// </summary>
        private CkSsl3KeyMatOut _returnedKeyMaterial = null;

        /// <summary>
        /// Resulting key handles and initialization vectors after performing a DeriveKey method
        /// </summary>
        public CkSsl3KeyMatOut ReturnedKeyMaterial
        {
            get
            {
                if (this._disposed)
                    throw new ObjectDisposedException(this.GetType().FullName);

                if (_returnedKeyMaterial == null)
                {
                    if (Platform.UnmanagedLongSize == 4)
                        _returnedKeyMaterial = (Platform.StructPackingSize == 0) ? new CkSsl3KeyMatOut(_params40.ReturnedKeyMaterial) : new CkSsl3KeyMatOut(_params41.ReturnedKeyMaterial);
                    else
                        _returnedKeyMaterial = (Platform.StructPackingSize == 0) ? new CkSsl3KeyMatOut(_params80.ReturnedKeyMaterial) : new CkSsl3KeyMatOut(_params81.ReturnedKeyMaterial);

                    // Since now it is the caller's responsibility to dispose returned key material
                    _returnedKeyMaterialLeftInstance = true;
                }

                return _returnedKeyMaterial;
            }
        }

        /// <summary>
        /// Client's and server's random data information
        /// </summary>
        private CkSsl3RandomData _randomInfo = null;

        /// <summary>
        /// Initializes a new instance of the CkSsl3KeyMatParams class.
        /// </summary>
        /// <param name='macSizeInBits'>The length (in bits) of the MACing keys agreed upon during the protocol handshake phase</param>
        /// <param name='keySizeInBits'>The length (in bits) of the secret keys agreed upon during the protocol handshake phase</param>
        /// <param name='ivSizeInBits'>The length (in bits) of the IV agreed upon during the protocol handshake phase or if no IV is required, the length should be set to 0</param>
        /// <param name='isExport'>Flag indicating whether the keys have to be derived for an export version of the protocol</param>
        /// <param name='randomInfo'>Client's and server's random data information</param>
        public CkSsl3KeyMatParams(ulong macSizeInBits, ulong keySizeInBits, ulong ivSizeInBits, bool isExport, CkSsl3RandomData randomInfo)
        {
            if (randomInfo == null)
                throw new ArgumentNullException("randomInfo");

            // Keep reference to randomInfo so GC will not free it while this object exists
            _randomInfo = randomInfo;

            if (Platform.UnmanagedLongSize == 4)
            {
                if (Platform.StructPackingSize == 0)
                    _params40 = new HighLevelAPI40.MechanismParams.CkSsl3KeyMatParams(Convert.ToUInt32(macSizeInBits), Convert.ToUInt32(keySizeInBits), Convert.ToUInt32(ivSizeInBits), isExport, _randomInfo._params40);
                else
                    _params41 = new HighLevelAPI41.MechanismParams.CkSsl3KeyMatParams(Convert.ToUInt32(macSizeInBits), Convert.ToUInt32(keySizeInBits), Convert.ToUInt32(ivSizeInBits), isExport, _randomInfo._params41);
            }
            else
            {
                if (Platform.StructPackingSize == 0)
                    _params80 = new HighLevelAPI80.MechanismParams.CkSsl3KeyMatParams(macSizeInBits, keySizeInBits, ivSizeInBits, isExport, _randomInfo._params80);
                else
                    _params81 = new HighLevelAPI81.MechanismParams.CkSsl3KeyMatParams(macSizeInBits, keySizeInBits, ivSizeInBits, isExport, _randomInfo._params81);
            }
        }
        
        #region IMechanismParams

        /// <summary>
        /// Returns managed object that can be marshaled to an unmanaged block of memory
        /// </summary>
        /// <returns>A managed object holding the data to be marshaled. This object must be an instance of a formatted class.</returns>
        public object ToMarshalableStructure()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (Platform.UnmanagedLongSize == 4)
                return (Platform.StructPackingSize == 0) ? _params40.ToMarshalableStructure() : _params41.ToMarshalableStructure();
            else
                return (Platform.StructPackingSize == 0) ? _params80.ToMarshalableStructure() : _params81.ToMarshalableStructure();
        }
        
        #endregion
        
        #region IDisposable
        
        /// <summary>
        /// Disposes object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Disposes object
        /// </summary>
        /// <param name="disposing">Flag indicating whether managed resources should be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // Dispose managed objects
                    if (_params40 != null)
                    {
                        _params40.Dispose();
                        _params40 = null;
                    }

                    if (_params41 != null)
                    {
                        _params41.Dispose();
                        _params41 = null;
                    }

                    if (_params80 != null)
                    {
                        _params80.Dispose();
                        _params80 = null;
                    }

                    if (_params81 != null)
                    {
                        _params81.Dispose();
                        _params81 = null;
                    }

                    if (_returnedKeyMaterialLeftInstance == false)
                    {
                        if (_returnedKeyMaterial != null)
                        {
                            _returnedKeyMaterial.Dispose();
                            _returnedKeyMaterial = null;
                        }
                    }
                }
                
                // Dispose unmanaged objects
                
                _disposed = true;
            }
        }
        
        /// <summary>
        /// Class destructor that disposes object if caller forgot to do so
        /// </summary>
        ~CkSsl3KeyMatParams()
        {
            Dispose(false);
        }
        
        #endregion
    }
}
