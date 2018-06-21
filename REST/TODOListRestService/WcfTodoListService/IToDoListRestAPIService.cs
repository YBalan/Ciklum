﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfTodoListService.DataModel;

namespace WcfTodoListService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IToDoListRestAPIService
    {
        [OperationContract]
        [WebGet(UriTemplate="/getdata/{value}")]//(UriTemplate = "/getdata", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        string GetData(string value);

        #region GET Methods

        [OperationContract]
        [WebGet(UriTemplate = "/lists", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<TodoList> GetLists();

        [OperationContract]
        [FaultContract(typeof(RESTAPIExceptionData))]
        [WebGet(UriTemplate = "/list/{id}", ResponseFormat = WebMessageFormat.Json)]
        TodoList GetList(string id);

        #endregion

        #region POST Methods
        #endregion
    }   
}
