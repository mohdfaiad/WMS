﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using WMS_Interface;
using WMS_Database;


namespace WMS_Kernel
{
    public partial class CellSetView : ChildViewBase
    {
        public bool IsModify = false;
        StorageViewPresenter presenter=null;
        private string houseName = "";
        private int rowth = 1;
        private Dictionary<string, string> areaColor = new Dictionary<string, string>();
        WH_AreaBll bllArea = new WH_AreaBll();
        WH_WareHouseBll bllWareHouse = new WH_WareHouseBll();
        public CellSetView(string houseName, int rowth, StorageViewPresenter presenter, List<int> colList, List<int> layerList,List<string> cellPosList)
        {
            InitializeComponent();
            this.Text = houseName +"-"+rowth +"排-货位批量设置";
            this.presenter = presenter;
            this.houseName = houseName;
            this.rowth = rowth;
            IniLayerList(layerList);
            IniLayerListArea(layerList);
            IniColList(colList);
            IniColListArea(colList);
            IniHouseAreaList();
            InitPosTypeList(cellPosList);
        }
        public void InitPosTypeList(List<string> posList)
        {
            if (posList == null)
            {
                return;
            }
            this.cb_CellPos.Items.Clear();
            this.cb_CellPos.Items.Add("所有");
            foreach (string pos in posList)
            {
                this.cb_CellPos.Items.Add(pos);
            }
            if (posList.Count > 0)
            {
                this.cb_CellPos.SelectedIndex = 0;
            }
        }
        private void IniLayerListArea( List<int> layerList)
        {
            //this.cb_LayerListArea.Items.Clear();
            this.cb_startLayer.Items.Clear();
            this.cb_EndLayer.Items.Clear();

            for (int i = 0; i < layerList.Count; i++)
            {
                //this.cb_LayerListArea.Items.Add(layerList[i]);
                this.cb_startLayer.Items.Add(layerList[i]);
                this.cb_EndLayer.Items.Add(layerList[i]);

            }

            if (this.cb_startLayer.Items.Count > 0)
            {
                //this.cb_LayerListArea.SelectedIndex = 0;
                this.cb_startLayer.SelectedIndex = 0;
                this.cb_EndLayer.SelectedIndex = 0;
            }
        }
        private void IniLayerList( List<int> layerList)
        {
            this.cb_LayerList.Items.Clear();
            for(int i=0;i<layerList.Count;i++)
            {
                this.cb_LayerList.Items.Add(layerList[i]);

            }
             if( this.cb_LayerList.Items.Count>0)
             {
                 this.cb_LayerList.SelectedIndex = 0;
             }
        }

        private void IniColListArea(List<int> colList)
        {
            this.cb_ColListSTArea.Items.Clear();
            this.cb_ColListEDArea.Items.Clear();

            for (int i = 0; i < colList.Count; i++)
            {
                this.cb_ColListSTArea.Items.Add(colList[i]);
                this.cb_ColListEDArea.Items.Add(colList[i]);
            }
            if (this.cb_ColListSTArea.Items.Count > 0)
            {
                this.cb_ColListSTArea.SelectedIndex = 0;

            }
            if (this.cb_ColListEDArea.Items.Count > 0)
            {
                this.cb_ColListEDArea.SelectedIndex = 0;
            }

        }
        private void IniHouseAreaList()
        {
            this.cb_HouseArea.Items.Clear();

            WH_WareHouseModel house  = bllWareHouse.GetModelByName(houseName);
            if(house == null)
            {
                return;
            }
            List<WH_AreaModel> areaList = bllArea.GetModels(house.WareHouse_ID);

           // for(int i=0;i<Enum.GetNames(typeof(EnumLogicArea)).Length;i++)
            for (int i = 0; i < areaList.Count(); i++)
            {
                if(areaList[i].Area_Name == "暂存区")
                {
                    continue;
                }
                string areaName = areaList[i].Area_Name;//Enum.GetNames(typeof(EnumLogicArea))[i];
                this.cb_HouseArea.Items.Add(areaName);
            }
            if(this.cb_HouseArea.Items.Count>0)
            {
                this.cb_HouseArea.SelectedIndex = 0;
            }
        }

        private void IniColList(List<int> colList)
        {
            this.cb_ColSTList.Items.Clear();
            this.cb_ColEDList.Items.Clear();
            for (int i = 0; i < colList.Count; i++)
            {
                this.cb_ColSTList.Items.Add(colList[i]);
                this.cb_ColEDList.Items.Add(colList[i]);

            }
            if (this.cb_ColSTList.Items.Count > 0)
            {
                this.cb_ColSTList.SelectedIndex = 0;
            }
            if (this.cb_ColEDList.Items.Count > 0)
            {
                this.cb_ColEDList.SelectedIndex = 0;
            }
        }
        private void bt_UseGs_Click(object sender, EventArgs e)
        {
           
            if(this.rb_singleCol.Checked == true)
            {
                int stCol = int.Parse(this.cb_ColSTList.Text);
                int edCol = int.Parse(this.cb_ColEDList.Text);
                if(edCol<stCol)
                {
                    MessageBox.Show("您的起止列设置错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
               
                }

                this.presenter.SetMulColCellEnabled(this.houseName,true, this.rowth, stCol, edCol);
            }
            else if(this.rb_SingleLayer.Checked == true)
            {
                int layerth = int.Parse(this.cb_LayerList.Text);
                this.presenter.SetSingleLayerCellEnabled(this.houseName, true, this.rowth,int.Parse(this.cb_LayerList.Text),this.cb_CellPos.Text);
            }
            this.IsModify = true;
             
        }

        private void bt_GsFobit_Click(object sender, EventArgs e)
        {
          
            if (this.rb_singleCol.Checked == true)
            {
                int stCol = int.Parse(this.cb_ColSTList.Text);
                int edCol = int.Parse(this.cb_ColEDList.Text);
                if (edCol < stCol)
                {
                    MessageBox.Show("您的起止列设置错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }

                this.presenter.SetMulColCellEnabled(this.houseName, false, this.rowth, stCol, edCol);
            }
            else if (this.rb_SingleLayer.Checked == true)
            {
                int layerth = int.Parse(this.cb_LayerList.Text);
                this.presenter.SetSingleLayerCellEnabled(this.houseName, false, this.rowth,int.Parse(this.cb_LayerList.Text),this.cb_CellPos.Text);
            }
            this.IsModify = true;
           
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            this.IsModify = false;
            this.Close();
        }
 

        private void bt_AreaSet_Click(object sender, EventArgs e)
        {
          
                int startCol = int.Parse(this.cb_ColListSTArea.Text);
                int endCol = int.Parse(this.cb_ColListEDArea.Text);
                int startLayer = int.Parse(this.cb_startLayer.Text);
                int endLayer = int.Parse(this.cb_EndLayer.Text);

                if (endCol < startCol)
                {
                    MessageBox.Show("您的起止列设置错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (endLayer < startLayer)
                {
                    MessageBox.Show("您的起止层设置错误！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.presenter.SetMulLayerMulColArea(this.houseName,this.cb_HouseArea.Text, this.rowth, startCol, endCol,startLayer,endLayer);
           
          
        }

 
        //public static bool LoadColorCfg(ref string reStr)
        //{
        //    try
        //    {
        //        CtlDBAccess.BLL.SysCfgBll sysCfgBll = new CtlDBAccess.BLL.SysCfgBll();
        //        CtlDBAccess.Model.SysCfgDBModel cfgModel = sysCfgBll.GetModel(SysCfg.SysCfgModel.SysCfgFileName);
        //        XElement root = null;

        //        if (cfgModel == null)
        //        {
        //            reStr = "系统配置不存在!";
        //            return false;
        //        }
        //        root = XElement.Parse(cfgModel.cfgFile);
        //        if (root == null)
        //        {
        //            reStr = "系统配置不存在!";
        //            return false;
        //        }
        //        IEnumerable<XElement>  houseAreaColorSet = root.Element("sysSet").Elements("HouseAreaColorSet");
        //        if (houseAreaColorSet== null)
        //        {
        //            reStr = "系统逻辑库存颜色信息不存在!";
        //            return false;
        //        }
              
        //       foreach(XElement element in houseAreaColorSet)
        //       {
        //           string houseName = element.Attribute("HouseName").Value;
        //           string areaName = element.Attribute("HouseArea").Value;
        //           string rgb = element.Value;
        //       }
        //        //XElement root = XElement.Load(xmlCfgFile);
        //        XElement asrsStoreCfgXE = root.Element("sysSet").Element("AsrsStoreCfg");
        //        asrsStoreCfgXE.Attribute("StoreTime").Value = AsrsStoreTime.ToString();
        //        XElement asrsBatchCfgXE = root.Element("sysSet").Element("AsrsBatchCfg");
        //        asrsBatchCfgXE.Attribute("HouseACheckin").Value = CheckinBatchDic["A1库房"];
        //        asrsBatchCfgXE.Attribute("HouseACheckout").Value = CheckoutBatchDic["A1库房"];
        //        asrsBatchCfgXE.Attribute("HouseBCheckin").Value = CheckinBatchDic["B1库房"];
        //        asrsBatchCfgXE.Attribute("HouseBCheckout").Value = CheckoutBatchDic["B1库房"];
        //        asrsBatchCfgXE.Attribute("HouseC1Checkout").Value = CheckoutBatchDic["C1库房"];
        //        asrsBatchCfgXE.Attribute("HouseC2Checkout").Value = CheckoutBatchDic["C2库房"];
        //        XElement asrsEnableXE = root.Element("sysSet").Element("AsrsEnableSet");
        //        // asrsEnableXE.Attribute("HouseEnabledA").Value = HouseEnabledA.ToString();
        //        //   asrsEnableXE.Attribute("HouseEnabledB").Value = HouseEnabledB.ToString();
        //        // root.Save(xmlCfgFile);
        //        if (cfgModel != null)
        //        {
        //            cfgModel.cfgFile = root.ToString();
        //            sysCfgBll.Update(cfgModel);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        reStr = ex.ToString();
        //        return false;
        //    }
        //}
        //public static bool LoadCfg(ref XElement root, ref string reStr)
        //{
        //    try
        //    {
        //        //CheckinBatchDic = new Dictionary<string, string>();
        //        //CheckoutBatchDic = new Dictionary<string, string>();
        //        //stepSeqs.Clear();
        //        ////投产绑定，一次高温，OCV1，二次绑定，二次高温，冷却，OCV2,OCV3,常温老化，OCV4,下线入库
        //        //stepSeqs.AddRange(new string[] { "PS-10", "PS-20", "PS-40", "PS-41", "PS-50", "PS-60", "PS-70", "PS-90", "PS-100", "PS-110", "PS-120" });

        //        CtlDBAccess.BLL.SysCfgBll sysCfgBll = new CtlDBAccess.BLL.SysCfgBll();
        //        CtlDBAccess.Model.SysCfgDBModel cfgModel = sysCfgBll.GetModel(SysCfg.SysCfgModel.SysCfgFileName);

        //        //SysCfgModel.CfgFile = cfgFile;
        //        if (cfgModel == null)
        //        {
        //            reStr = "系统配置不存在";
        //            return false;
        //        }
        //        root = XElement.Parse(cfgModel.cfgFile);
        //        if (root == null)
        //        {
        //            reStr = "系统配置不存在!";
        //            return false;
        //        }


        //        XElement asrsStoreCfgXE = root.Element("sysSet").Element("AsrsStoreCfg");
        //        AsrsStoreTime = float.Parse(asrsStoreCfgXE.Attribute("StoreTime").Value);

        //        XElement runModeXE = root.Element("sysSet").Element("RunMode");
        //        string simStr = runModeXE.Attribute("sim").Value.ToString().ToUpper();
        //        if (simStr == "TRUE")
        //        {
        //            SimMode = true;
        //        }
        //        else
        //        {
        //            SimMode = false;
        //        }
        //        if (runModeXE.Attribute("RfidSimMode") != null)
        //        {
        //            string strRfidSim = runModeXE.Attribute("RfidSimMode").Value.ToString().ToUpper();
        //            if (strRfidSim == "TRUE")
        //            {
        //                RfidSimMode = true;
        //            }
        //            else
        //            {
        //                RfidSimMode = false;
        //            }
        //        }
        //        if (runModeXE.Attribute("UnBindedMode") != null)
        //        {
        //            string unbindedStr = runModeXE.Attribute("UnBindedMode").Value.ToString().ToUpper();
        //            if (unbindedStr == "TRUE")
        //            {
        //                UnbindMode = true;
        //            }
        //            else
        //            {
        //                UnbindMode = false;
        //            }
        //        }
        //        //if(root.Element("sysSet").Element("AsrsBatchSet") != null && 
        //        //    root.Element("sysSet").Element("AsrsBatchSet").Element("CheckInBatch") != null)
        //        //{

        //        //}
        //        XElement asrsBatchCfgXE = root.Element("sysSet").Element("AsrsBatchCfg");
        //        CheckinBatchDic["A1库房"] = asrsBatchCfgXE.Attribute("HouseACheckin").Value.ToString();
        //        CheckinBatchDic["B1库房"] = asrsBatchCfgXE.Attribute("HouseBCheckin").Value.ToString();
        //        CheckinBatchDic["C1库房"] = asrsBatchCfgXE.Attribute("HouseC1Checkin").Value.ToString();
        //        CheckinBatchDic["C2库房"] = asrsBatchCfgXE.Attribute("HouseC2Checkin").Value.ToString(); ;

        //        CheckoutBatchDic["A1库房"] = asrsBatchCfgXE.Attribute("HouseACheckout").Value.ToString();
        //        CheckoutBatchDic["B1库房"] = asrsBatchCfgXE.Attribute("HouseBCheckout").Value.ToString();
        //        CheckoutBatchDic["C1库房"] = asrsBatchCfgXE.Attribute("HouseC1Checkout").Value.ToString();
        //        CheckoutBatchDic["C2库房"] = asrsBatchCfgXE.Attribute("HouseC2Checkout").Value.ToString();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        reStr = ex.ToString();
        //        return false;
        //    }

        //}
    }
}
