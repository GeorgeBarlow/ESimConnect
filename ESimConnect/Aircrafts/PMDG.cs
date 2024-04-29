using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ESimConnect.Types;
using Microsoft.FlightSimulator.SimConnect;

namespace ESimConnect.Aircrafts
{
    public static class PMDG
    {
        public enum DATA_REQUEST_ID
        {
            AIR_PATH_REQUEST,
            CONTROL_REQUEST,
            DATA_REQUEST,
            CDU0_REQUEST,
            CDU1_REQUEST,
            CDU2_REQUEST
        }

        public static void RegisterPMDGNG3DataEvents(dynamic simConnect)
        {
            CreatePMDGEvents<PMDG_NG3_SDK.PMDGEvents>(simConnect);

            AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_Data>(simConnect, PMDG_NG3_SDK.DATA_NAME, PMDG_NG3_SDK.PMDG_NG3.DATA_ID, PMDG_NG3_SDK.PMDG_NG3.DATA_DEFINITION, DATA_REQUEST_ID.DATA_REQUEST);
            AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_CDU_Screen>(simConnect, PMDG_NG3_SDK.CDU_0_NAME, PMDG_NG3_SDK.PMDG_NG3.CDU_0_ID, PMDG_NG3_SDK.PMDG_NG3.CDU_0_DEFINITION, DATA_REQUEST_ID.CDU0_REQUEST);
            AssociatePMDGData<PMDG_NG3_SDK.PMDG_NG3_CDU_Screen>(simConnect, PMDG_NG3_SDK.CDU_1_NAME, PMDG_NG3_SDK.PMDG_NG3.CDU_1_ID, PMDG_NG3_SDK.PMDG_NG3.CDU_1_DEFINITION, DATA_REQUEST_ID.CDU1_REQUEST);

        }

        private static void CreatePMDGEvents<T>(SimConnect simConnect) where T : Enum
        {
            // Map the PMDG Events to SimConnect
            foreach (T eventid in Enum.GetValues(typeof(T)))
            {
                simConnect.MapClientEventToSimEvent(eventid, "#" + Convert.ChangeType(eventid, eventid.GetTypeCode()).ToString());
            }
        }

        private static void AssociatePMDGData<T>(SimConnect simConnect, string clientDataName, Enum clientDataID, Enum definitionID, Enum requestID) where T : struct
        {
            // Associate an ID with the PMDG data area name
            simConnect.MapClientDataNameToID(clientDataName, clientDataID);
            // Define the data area structure - this is a required step
            simConnect.AddToClientDataDefinition(definitionID, 0, (uint)Marshal.SizeOf<T>(), 0, 0);
            // Register the data area structure
            simConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, T>(definitionID);
            // Sign up for notification of data change
            simConnect.RequestClientData(
                clientDataID,
                requestID,
                definitionID,
                SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                0,
                0,
                0);

            simConnect.MapClientDataNameToID(PMDG_NG3_SDK.CONTROL_NAME, PMDG_NG3_SDK.PMDG_NG3.CONTROL_ID);
            simConnect.AddToClientDataDefinition(PMDG_NG3_SDK.PMDG_NG3.CONTROL_DEFINITION, 0, (uint)Marshal.SizeOf<PMDG_NG3_SDK.PMDG_NG3_Control>(), 0, 0);
            simConnect.RequestClientData(PMDG_NG3_SDK.PMDG_NG3.CONTROL_ID, DATA_REQUEST_ID.CONTROL_REQUEST, PMDG_NG3_SDK.PMDG_NG3.CONTROL_DEFINITION, SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET, SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED, 0, 0,0);


        }
    }
}
