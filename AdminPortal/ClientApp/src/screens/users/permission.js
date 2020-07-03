import MultiSelect from "@kenshooui/react-multi-select";
import "@kenshooui/react-multi-select/dist/style.css"
import React from "react";
import Select from 'react-select';
import * as Actions from '../../libs/actions';
import * as UserGroupService from '../../services/userGroupService';
import * as UserService from '../../services/usersService';

const Features = [
  { id: 1, name: "proposal", label: "Đề xuất" },
  { id: 2, name: "explanation", label: "Giải trình" },
  { id: 3, name: "survey", label: "Khảo sát" },
  { id: 4, name: "quote", label: "Báo Giá" },
  { id: 5, name: "audit", label: "Họp giá" },
  { id: 6, name: "bidPlan", label: "Kế Hoạch Thầu" },
  { id: 7, name: "negotiation", label: "BB Thương Thảo HĐ" },
  { id: 8, name: "decision", label: "QĐ Chọn Thầu" },
  { id: 9, name: "contract", label: "Hợp Đồng" },
  { id: 10, name: "deliveryReceipt", label: "BB Giao Nhận", },
  { id: 11, name: "acceptance", label: "BB Nghiệm Thu" },
]
const Headers = [
  "Đề xuất",
  "Giải trình",
  "Khảo sát",
  "Báo Giá",
  "Họp giá",
  "Kế Hoạch Thầu",
  "BB Thương Thảo HĐ",
  "QĐ Chọn Thầu",
  "Hợp Đồng",
  "BB Giao Nhận",
  "BB Nghiệm Thu"
]
const Group = [
  { value: 0, label: "Người dùng" },
  { value: 1, label: "Nhóm" },
]

export default class PermissionComp extends React.Component {

  constructor(props) {
    super(props);
    this.state = {

      typeGroupSelected: { value: 0, label: "Người dùng" },
  
  
      selectedItemValue: null,
      ItemOptions: [],
  
      listGroup: [],
  
      items: [],
      selectedItems: [],
      originValues: [],
      buttonDisable: true,
    };
  }

  
  componentDidMount() {
    this._getDataFunc = this.getListUserByName;
    this._getListDoubleColumnFunc = this.getListGroupMember;
    this._updateDataFunc = this.updateListGroupMember;
  }
  componentWillMount() {
    UserGroupService.GetAllUserWithCondition('', 100, 0)
      .then(objRespone => {
     
        if (objRespone.isSuccess ===true) {
          let listGroup = [];
          for (let item of objRespone.data) {
            listGroup.push({ id: item.groupID, value: item.groupID, label: item.groupName })
          }
          this.setState({ listGroup })
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      })
  }
  handleChangeGroup(typeGroupSelected) {
    let items = [];
    let ItemOptions = []
    if (typeGroupSelected.value ===0) {
      this._getDataFunc = this.getListUserByName;
      this._getListDoubleColumnFunc = this.getListGroupMember;
      this._updateDataFunc = this.updateListGroupMember;
      ItemOptions = this.state.listGroup
    }


    if (typeGroupSelected.value ===1) {
      this._getDataFunc = this.getListGroupByName;
      this._getListDoubleColumnFunc = this.getListPermission;
      this._updateDataFunc = this.updatePermission;
    }
    this._getDataFunc('');
    this.setState({ items, typeGroupSelected, ItemOptions, selectedItemValue: null, selectedItems:[],
      originValues: [], buttonDisable: true });
  }

  getListGroupMember(userID) {
    UserGroupService.getListGroupOfUser(userID)
    .then(objRespone => {
      if (objRespone.isSuccess ===true) {
        let originValues = [];
        for (let item of objRespone.data) {
          originValues.push({id:item.groupID, value: item.groupID, label: item.groupName })
        }
        this.setState({ items:this.state.listGroup, originValues, selectedItems: originValues })
      } else {
        Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
      }
    }) 
  }

  updateListGroupMember() {
    let selectedItemValue = (this.state.selectedItemValue || null) 
    if (selectedItemValue !==null) {
      let UserID = selectedItemValue.value
      let data = this.state.selectedItems 
      let request = [];

      for (let record of data) {
        request.push({ userID: UserID, groupID: record.value })
      }
      UserGroupService.updatelstGroupOfUser(UserID, request).then((Response) => {
        if (Response.isSuccess) {
          this.setState({ originValues: data, buttonDisable: true })
        }
      })
    }
  }

  getListPermission(groupID) {
    UserGroupService.getlstGroupPermission(groupID).then((respone) => {
      if (respone.isSuccess) {
        let originValues = [];
        for (let item of respone.data) {
          originValues.push({ id: item.feature, label: Headers[item.feature] })
        }
        this.setState({ originValues, selectedItems: originValues })
      }
    })
  }
  updatePermission() {
    let selectedItemValue = (this.state.selectedItemValue || null) 
    if (selectedItemValue !==null) {
      let groupId = selectedItemValue.value
      let data = this.state.selectedItems 
      let request = [];
      for (let record of data) {
        request.push({ groupID: groupId, feature: record.id })
      }
      UserGroupService.updateGroupPermission(groupId, request).then((Response) => {

        if (Response.isSuccess) {
          this.setState({ originValues: data, buttonDisable: true })
        }
      })
    }
  }


  onSelectDataItem(selectedItemValue) {
    if (selectedItemValue) {
      if (this.state.typeGroupSelected.value ===1) {
        this.setState({ items: Features })
      }
    }
    this.setState({ selectedItemValue }, () => {
      this._getListDoubleColumnFunc(selectedItemValue.value)
    });
  };


  handleChangeDoubleCells(selectedItems) {

    let buttonDisable = true
    if (JSON.stringify(selectedItems) !==JSON.stringify(this.state.originValues)) {
      buttonDisable = false
    } else {
      buttonDisable = true

    }
    this.setState({ selectedItems: selectedItems, buttonDisable });
  }

  getListUserByName(name) {
    UserService.GetAllUserWithCondition('', name, 100, 0)
      .then(objRespone => {
        if (objRespone.isSuccess ===true) {
          let ItemOptions = [];
          for (let item of objRespone.data) {
            ItemOptions.push({ value: item.userID, label: item.userName })
          }
          this.setState({ ItemOptions })
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      })
  }

  getListGroupByName(name) {
    UserGroupService.GetAllUserWithCondition(name, 100, 0)
      .then(objRespone => {
        if (objRespone.isSuccess ===true) {
          let ItemOptions = [];
          for (let item of objRespone.data) {
            ItemOptions.push({ value: item.groupID, label: item.groupName })
          }
          this.setState({ ItemOptions })
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      })
  }

  handleInputChangeSearch(newValue, force) {
    if (this.state.typeGroupSelected.value ===0) {
      if (newValue.length >= 1) {
        if (this.task) clearTimeout(this.task);
        if (this._getDataFunc) {
          this.task = setTimeout(() => {
            this._getDataFunc(newValue)
          }, 300);
        }
      }
    }
  };



  render() {
    let typeGroupSelected = this.state.typeGroupSelected
    let selectedItemValue = this.state.selectedItemValue

    let items = this.state.items;

    let selectedItems = this.state.selectedItems
    return (
      <React.Fragment>
        <div style={{ width: '100%', display: 'flex', paddingTop: 20, paddingBottom: 20 }}>
          <div>
            <Select
              placeholder={"name"}
              styles={customStyles}
              value={typeGroupSelected}
              onChange={(value) => { this.handleChangeGroup(value) }}
              options={Group}
            />
          </div>
          <div style={{ marginLeft: 20 }}>
            <Select
              key={"autoComplete select"}
              styles={customStyles}
              value={selectedItemValue}
              onInputChange={(value) => { this.handleInputChangeSearch(value, false) }}
              onChange={(value) => { this.onSelectDataItem(value) }}
              options={this.state.ItemOptions || []}
            />
          </div>
        </div>
        <div style={{ display: 'flex' }}>
          <div style={{ width: '70%' }}>
            <MultiSelect
              items={items}
              selectedItems={selectedItems}
              onChange={this.handleChangeDoubleCells.bind(this)}
            />
          </div>
          <div style={{ width: '28%' }}>
            <button type="button" style={{ marginLeft: 30, height: 40, width: 100 }}
              disabled={this.state.buttonDisable}
              className="btn btn-success"
              onClick={() => {
                if (this._updateDataFunc)
                  this._updateDataFunc();
                // this.addItem();
              }}
            > Lưu </button>
            <button type="button" style={{ marginLeft: 30, height: 40, width: 100 }} className="btn btn-danger"
              disabled={this.state.buttonDisable}
              onClick={() => {
                this.setState({
                  buttonDisable: true,
                  selectedItems: this.state.originValues
                })
              }}
            > Reset </button>
          </div>
        </div>
      </React.Fragment >
    );
  }
};

const customStyles = {
  placeholder: () => ({
    margin: 0,
    color: '#aaa'
  }),
  indicatorSeparator: () => ({
    color: '#fff'
  }),

  option: (provided, state) => ({
    ...provided,
    fontSize: 12,
    lineHeight: '12px',
    fontFamily: 'roboto',
    marginTop: 4
  }),
  control: () => ({
    display: 'flex',
    width: 175,
    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
    height: 32,
    bordeRadius: 3,
    paddingLeft: 5,
    fontSize: 12,
    lineHeight: '12px',
    fontFamily: 'roboto',
  }),
  singleValue: (provided, state) => {
    const opacity = state.isDisabled ? 0.5 : 1;
    const transition = 'opacity 300ms';
    return { ...provided, opacity, transition };
  }
}