using HDUP3.HDUP.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HDUP3
{
    public partial class MainWindow : Window
    {

        public static String StoreJsonLocation;
        public static String DeviceJsonLocation;
        public static String ActionJsonLocation;
        public static String PasscodeJsonLocation;

        public static String ScriptFolderLocation;

        public static String SQL_Username;
        public static String SQL_Password;

        public static String RA_Username;
        public static String RA_Password;
        public static String RA_Browser; // Web-Browser (iexplorer, chrome, firefox)

        public MainWindow()
        {
            InitializeComponent();

            // Reset the error text lables
            lblStoreError.Content = "";
            lblDeviceError.Content = "";
            lblActionError.Content = "";

            if (File.Exists(@"H:\HDUP3\JSON\settings.json"))
            { //H:\HDUP3\JSON\settings.json
                Settings settings = (Settings)JsonHandler.Desteralize(@"H:\HDUP3\JSON\settings.json");

                StoreJsonLocation = settings.StoreJsonLocation;
                DeviceJsonLocation = settings.DeviceJsonLocation;
                ActionJsonLocation = settings.ActionJsonLocation;
                PasscodeJsonLocation = settings.PasscodeJsonLocation;

                ScriptFolderLocation = settings.ScriptFolderLocation;

                SQL_Username = settings.SQL_Username;
                SQL_Password = settings.SQL_Password;

                RA_Username = settings.Remotely_Username;
                RA_Password = settings.Remotely_Password;
                RA_Browser = settings.Remotely_Browser;
                
            }

            if (File.Exists(PasscodeJsonLocation))
            {
                this.Title = "HDUP3 - " + JsonHandler.GetPasscode();
            }

            if (File.Exists(StoreJsonLocation))
            {
                boxStore.Items.Clear();

                JsonHandler.stores = (List<Store>) JsonHandler.Desteralize(StoreJsonLocation);

                foreach (Store store in JsonHandler.stores)
                {
                    if (!store.Relocation)
                        boxStore.Items.Add(store.ID);
                }
            }

            if (File.Exists(DeviceJsonLocation))
                JsonHandler.devices = (List<Device>) JsonHandler.Desteralize(DeviceJsonLocation);

            if (File.Exists(ActionJsonLocation))
                JsonHandler.actions = (List<Actions>) JsonHandler.Desteralize(ActionJsonLocation);

        }

        private void chkRelocation_Checked(object sender, RoutedEventArgs e)
        {
            boxStore.Text = "";

            boxStore.Items.Clear();
            boxDevice.Items.Clear();
            boxAction.Items.Clear();

            lblStoreError.Content = "";
            lblDeviceError.Content = "";
            lblActionError.Content = "";

            foreach (Store store in JsonHandler.stores)
            {
                if (chkRelocation.IsChecked.Value && store.Relocation)
                    boxStore.Items.Add(store.ID);
                else if (!chkRelocation.IsChecked.Value && !store.Relocation)
                    boxStore.Items.Add(store.ID);
            }
        }

        private void chkRelocation_Unchecked(object sender, RoutedEventArgs e)
        {
            boxStore.Text = "";

            boxStore.Items.Clear();
            boxDevice.Items.Clear();
            boxAction.Items.Clear();

            lblStoreError.Content = "";
            lblDeviceError.Content = "";
            lblActionError.Content = "";

            foreach (Store store in JsonHandler.stores)
            {
                if (!store.Relocation)
                    boxStore.Items.Add(store.ID);
            }
        }

        private void boxStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxStore.SelectedItem != null)
                if (JsonHandler.GetStore(int.Parse(boxStore.SelectedItem.ToString()), chkRelocation.IsChecked.Value) != null)
                {
                    Store store = JsonHandler.GetStore(int.Parse(boxStore.SelectedItem.ToString()), chkRelocation.IsChecked.Value);

                    List<int> storesDevices = store.getDevicesID();


                    boxDevice.Items.Clear();

                    lblStoreError.Content = "";
                    lblDeviceError.Content = "";
                    lblActionError.Content = "";

                    foreach (Device device in JsonHandler.devices)
                    {
                        if (storesDevices.Contains(device.ID))
                        {
                            boxDevice.Items.Add(device.ID + " - " + device.Name);
                        }
                    }
                }
                else
                    System.Diagnostics.Debug.Write("Store doesn't exist! ID=" + boxStore.SelectedItem.ToString() + " Relocation=" + chkRelocation.IsChecked.Value);
        }

        private void boxDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (boxDevice.SelectedItem != null)
            {
                int ID = int.Parse(Regex.Replace(boxDevice.SelectedItem.ToString(), "[^.0-9]", ""));

                if (JsonHandler.GetDevice(ID) != null)
                {
                    Device device = JsonHandler.GetDevice(ID);

                    List<int> deviceActions = device.getActionsID();

                    boxAction.Items.Clear();

                    lblStoreError.Content = "";
                    lblDeviceError.Content = "";
                    lblActionError.Content = "";

                    foreach (Actions action in JsonHandler.actions)
                    {
                        if (deviceActions.Contains(action.ID))
                        {
                            boxAction.Items.Add(action.ID + " - " + action.Name);
                        }
                    }
                }
            }
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            if (boxAction.SelectedItem == null) { lblActionError.Content = "Not a valid input!"; }
            if (boxDevice.SelectedItem == null) { lblDeviceError.Content = "Not a valid input!"; }
            if (boxStore.SelectedItem == null) { lblStoreError.Content = "Not a valid input!"; }

            if (boxAction.SelectedItem != null && boxDevice.SelectedItem != null && boxStore.SelectedItem != null)
            {
                
                int ActionID = int.Parse(Regex.Replace(boxAction.SelectedItem.ToString(), "[^.0-9]", ""));
                int DeviceID = int.Parse(Regex.Replace(boxDevice.SelectedItem.ToString(), "[^.0-9]", ""));
                int StoreID = int.Parse(Regex.Replace(boxStore.SelectedItem.ToString(), "[^.0-9]", ""));

                Boolean Relocation = chkRelocation.IsChecked.Value;

                JsonHandler.GetAction(ActionID).Run(StoreID, Relocation, DeviceID);

                boxStore.Text = "";
                boxDevice.Items.Clear();
                boxAction.Items.Clear();
                 
                lblStoreError.Content = "";
                lblDeviceError.Content = "";
                lblActionError.Content = "";
            }
        }

        private void EnterKeyAsTab(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //SendKeys.SendWait("{TAB}");
            }
        }
    }
}
