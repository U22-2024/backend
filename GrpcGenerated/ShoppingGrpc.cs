// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: shopping/v1/shopping.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Shopping.V1 {
  public static partial class ShoppingService
  {
    static readonly string __ServiceName = "shopping.v1.ShoppingService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.GetShoppingListRequest> __Marshaller_shopping_v1_GetShoppingListRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.GetShoppingListRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.GetShoppingListResponse> __Marshaller_shopping_v1_GetShoppingListResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.GetShoppingListResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.CreateShoppingItemRequest> __Marshaller_shopping_v1_CreateShoppingItemRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.CreateShoppingItemRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.CreateShoppingItemResponse> __Marshaller_shopping_v1_CreateShoppingItemResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.CreateShoppingItemResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.UpdateShoppingItemRequest> __Marshaller_shopping_v1_UpdateShoppingItemRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.UpdateShoppingItemRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.UpdateShoppingItemResponse> __Marshaller_shopping_v1_UpdateShoppingItemResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.UpdateShoppingItemResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.DeleteShoppingItemRequest> __Marshaller_shopping_v1_DeleteShoppingItemRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.DeleteShoppingItemRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Shopping.V1.DeleteShoppingItemResponse> __Marshaller_shopping_v1_DeleteShoppingItemResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Shopping.V1.DeleteShoppingItemResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shopping.V1.GetShoppingListRequest, global::Shopping.V1.GetShoppingListResponse> __Method_GetShoppingList = new grpc::Method<global::Shopping.V1.GetShoppingListRequest, global::Shopping.V1.GetShoppingListResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetShoppingList",
        __Marshaller_shopping_v1_GetShoppingListRequest,
        __Marshaller_shopping_v1_GetShoppingListResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shopping.V1.CreateShoppingItemRequest, global::Shopping.V1.CreateShoppingItemResponse> __Method_CreateShoppingItem = new grpc::Method<global::Shopping.V1.CreateShoppingItemRequest, global::Shopping.V1.CreateShoppingItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateShoppingItem",
        __Marshaller_shopping_v1_CreateShoppingItemRequest,
        __Marshaller_shopping_v1_CreateShoppingItemResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shopping.V1.UpdateShoppingItemRequest, global::Shopping.V1.UpdateShoppingItemResponse> __Method_UpdateShoppingItem = new grpc::Method<global::Shopping.V1.UpdateShoppingItemRequest, global::Shopping.V1.UpdateShoppingItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateShoppingItem",
        __Marshaller_shopping_v1_UpdateShoppingItemRequest,
        __Marshaller_shopping_v1_UpdateShoppingItemResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Shopping.V1.DeleteShoppingItemRequest, global::Shopping.V1.DeleteShoppingItemResponse> __Method_DeleteShoppingItem = new grpc::Method<global::Shopping.V1.DeleteShoppingItemRequest, global::Shopping.V1.DeleteShoppingItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DeleteShoppingItem",
        __Marshaller_shopping_v1_DeleteShoppingItemRequest,
        __Marshaller_shopping_v1_DeleteShoppingItemResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Shopping.V1.ShoppingReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of ShoppingService</summary>
    [grpc::BindServiceMethod(typeof(ShoppingService), "BindService")]
    public abstract partial class ShoppingServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shopping.V1.GetShoppingListResponse> GetShoppingList(global::Shopping.V1.GetShoppingListRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shopping.V1.CreateShoppingItemResponse> CreateShoppingItem(global::Shopping.V1.CreateShoppingItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shopping.V1.UpdateShoppingItemResponse> UpdateShoppingItem(global::Shopping.V1.UpdateShoppingItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Shopping.V1.DeleteShoppingItemResponse> DeleteShoppingItem(global::Shopping.V1.DeleteShoppingItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for ShoppingService</summary>
    public partial class ShoppingServiceClient : grpc::ClientBase<ShoppingServiceClient>
    {
      /// <summary>Creates a new client for ShoppingService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ShoppingServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ShoppingService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ShoppingServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ShoppingServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ShoppingServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.GetShoppingListResponse GetShoppingList(global::Shopping.V1.GetShoppingListRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetShoppingList(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.GetShoppingListResponse GetShoppingList(global::Shopping.V1.GetShoppingListRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetShoppingList, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.GetShoppingListResponse> GetShoppingListAsync(global::Shopping.V1.GetShoppingListRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetShoppingListAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.GetShoppingListResponse> GetShoppingListAsync(global::Shopping.V1.GetShoppingListRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetShoppingList, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.CreateShoppingItemResponse CreateShoppingItem(global::Shopping.V1.CreateShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateShoppingItem(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.CreateShoppingItemResponse CreateShoppingItem(global::Shopping.V1.CreateShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateShoppingItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.CreateShoppingItemResponse> CreateShoppingItemAsync(global::Shopping.V1.CreateShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateShoppingItemAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.CreateShoppingItemResponse> CreateShoppingItemAsync(global::Shopping.V1.CreateShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateShoppingItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.UpdateShoppingItemResponse UpdateShoppingItem(global::Shopping.V1.UpdateShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateShoppingItem(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.UpdateShoppingItemResponse UpdateShoppingItem(global::Shopping.V1.UpdateShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_UpdateShoppingItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.UpdateShoppingItemResponse> UpdateShoppingItemAsync(global::Shopping.V1.UpdateShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateShoppingItemAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.UpdateShoppingItemResponse> UpdateShoppingItemAsync(global::Shopping.V1.UpdateShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_UpdateShoppingItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.DeleteShoppingItemResponse DeleteShoppingItem(global::Shopping.V1.DeleteShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteShoppingItem(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Shopping.V1.DeleteShoppingItemResponse DeleteShoppingItem(global::Shopping.V1.DeleteShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_DeleteShoppingItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.DeleteShoppingItemResponse> DeleteShoppingItemAsync(global::Shopping.V1.DeleteShoppingItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteShoppingItemAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Shopping.V1.DeleteShoppingItemResponse> DeleteShoppingItemAsync(global::Shopping.V1.DeleteShoppingItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_DeleteShoppingItem, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override ShoppingServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ShoppingServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(ShoppingServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetShoppingList, serviceImpl.GetShoppingList)
          .AddMethod(__Method_CreateShoppingItem, serviceImpl.CreateShoppingItem)
          .AddMethod(__Method_UpdateShoppingItem, serviceImpl.UpdateShoppingItem)
          .AddMethod(__Method_DeleteShoppingItem, serviceImpl.DeleteShoppingItem).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ShoppingServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetShoppingList, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shopping.V1.GetShoppingListRequest, global::Shopping.V1.GetShoppingListResponse>(serviceImpl.GetShoppingList));
      serviceBinder.AddMethod(__Method_CreateShoppingItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shopping.V1.CreateShoppingItemRequest, global::Shopping.V1.CreateShoppingItemResponse>(serviceImpl.CreateShoppingItem));
      serviceBinder.AddMethod(__Method_UpdateShoppingItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shopping.V1.UpdateShoppingItemRequest, global::Shopping.V1.UpdateShoppingItemResponse>(serviceImpl.UpdateShoppingItem));
      serviceBinder.AddMethod(__Method_DeleteShoppingItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shopping.V1.DeleteShoppingItemRequest, global::Shopping.V1.DeleteShoppingItemResponse>(serviceImpl.DeleteShoppingItem));
    }

  }
}
#endregion
