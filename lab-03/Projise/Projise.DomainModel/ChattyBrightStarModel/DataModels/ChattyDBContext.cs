﻿ 

// -----------------------------------------------------------------------
// <autogenerated>
//    This code was generated from a template.
//
//    Changes to this file may cause incorrect behaviour and will be lost
//    if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using BrightstarDB.Client;
using BrightstarDB.EntityFramework;

using System.Text;
using System.Threading.Tasks;

namespace Projise.DomainModel.ChattyBrightStarModel.DataModels 
{
    public partial class ChattyDBContext : BrightstarEntityContext {
    	
    	static ChattyDBContext() 
    	{
    		var provider = new ReflectionMappingProvider();
    		provider.AddMappingsForType(EntityMappingStore.Instance, typeof(Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage));
    		EntityMappingStore.Instance.SetImplMapping<Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage, Projise.DomainModel.ChattyBrightStarModel.DataModels.ChatMessage>();
    	}
    	
    	/// <summary>
    	/// Initialize a new entity context using the specified BrightstarDB
    	/// Data Object Store connection
    	/// </summary>
    	/// <param name="dataObjectStore">The connection to the BrightstarDB Data Object Store that will provide the entity objects</param>
    	public ChattyDBContext(IDataObjectStore dataObjectStore) : base(dataObjectStore)
    	{
    		InitializeContext();
    	}
    
    	/// <summary>
    	/// Initialize a new entity context using the specified Brightstar connection string
    	/// </summary>
    	/// <param name="connectionString">The connection to be used to connect to an existing BrightstarDB store</param>
    	/// <param name="enableOptimisticLocking">OPTIONAL: If set to true optmistic locking will be applied to all entity updates</param>
        /// <param name="updateGraphUri">OPTIONAL: The URI identifier of the graph to be updated with any new triples created by operations on the store. If
        /// not defined, the default graph in the store will be updated.</param>
        /// <param name="datasetGraphUris">OPTIONAL: The URI identifiers of the graphs that will be queried to retrieve entities and their properties.
        /// If not defined, all graphs in the store will be queried.</param>
        /// <param name="versionGraphUri">OPTIONAL: The URI identifier of the graph that contains version number statements for entities. 
        /// If not defined, the <paramref name="updateGraphUri"/> will be used.</param>
    	public ChattyDBContext(
    	    string connectionString, 
    		bool? enableOptimisticLocking=null,
    		string updateGraphUri = null,
    		IEnumerable<string> datasetGraphUris = null,
    		string versionGraphUri = null
        ) : base(connectionString, enableOptimisticLocking, updateGraphUri, datasetGraphUris, versionGraphUri)
    	{
    		InitializeContext();
    	}
    
    	/// <summary>
    	/// Initialize a new entity context using the specified Brightstar
    	/// connection string retrieved from the configuration.
    	/// </summary>
    	public ChattyDBContext() : base()
    	{
    		InitializeContext();
    	}
    	
    	/// <summary>
    	/// Initialize a new entity context using the specified Brightstar
    	/// connection string retrieved from the configuration and the
    	//  specified target graphs
    	/// </summary>
        /// <param name="updateGraphUri">The URI identifier of the graph to be updated with any new triples created by operations on the store. If
        /// set to null, the default graph in the store will be updated.</param>
        /// <param name="datasetGraphUris">The URI identifiers of the graphs that will be queried to retrieve entities and their properties.
        /// If set to null, all graphs in the store will be queried.</param>
        /// <param name="versionGraphUri">The URI identifier of the graph that contains version number statements for entities. 
        /// If set to null, the value of <paramref name="updateGraphUri"/> will be used.</param>
    	public ChattyDBContext(
    		string updateGraphUri,
    		IEnumerable<string> datasetGraphUris,
    		string versionGraphUri
    	) : base(updateGraphUri:updateGraphUri, datasetGraphUris:datasetGraphUris, versionGraphUri:versionGraphUri)
    	{
    		InitializeContext();
    	}
    	
    	private void InitializeContext() 
    	{
    		ChatMessages = 	new BrightstarEntitySet<Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage>(this);
    	}
    	
    	public IEntitySet<Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage> ChatMessages
    	{
    		get; private set;
    	}
    	
        public IEntitySet<T> EntitySet<T>() where T : class {
            var itemType = typeof(T);
            if (typeof(T).Equals(typeof(Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage))) {
                return (IEntitySet<T>)this.ChatMessages;
            }
            throw new InvalidOperationException(typeof(T).FullName + " is not a recognized entity interface type.");
        }
    
        } // end class ChattyDBContext
        
}
namespace Projise.DomainModel.ChattyBrightStarModel.DataModels 
{
    
    public partial class ChatMessage : BrightstarEntityObject, IChatMessage 
    {
    	public ChatMessage(BrightstarEntityContext context, BrightstarDB.Client.IDataObject dataObject) : base(context, dataObject) { }
    	public ChatMessage() : base() { }
    	public System.String Id { get {return GetKey(); } set { SetKey(value); } }
    	#region Implementation of Projise.DomainModel.ChattyBrightStarModel.DataModels.IChatMessage
    
    	public System.String Name
    	{
            		get { return GetRelatedProperty<System.String>("Name"); }
            		set { SetRelatedProperty("Name", value); }
    	}
    
    	public System.String Message
    	{
            		get { return GetRelatedProperty<System.String>("Message"); }
            		set { SetRelatedProperty("Message", value); }
    	}
    
    	public System.DateTime Date
    	{
            		get { return GetRelatedProperty<System.DateTime>("Date"); }
            		set { SetRelatedProperty("Date", value); }
    	}
    	#endregion
    }
}
