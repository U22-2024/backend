// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: event/v1/event_material.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Event.V1 {
  public static partial class EventMaterialService
  {
    static readonly string __ServiceName = "event.v1.EventMaterialService";

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
    static readonly grpc::Marshaller<global::Event.V1.PredictEventMaterialItemRequest> __Marshaller_event_v1_PredictEventMaterialItemRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictEventMaterialItemRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictEventMaterialItemResponse> __Marshaller_event_v1_PredictEventMaterialItemResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictEventMaterialItemResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictPositionsFromTextRequest> __Marshaller_event_v1_PredictPositionsFromTextRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictPositionsFromTextRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictPositionsFromTextResponse> __Marshaller_event_v1_PredictPositionsFromTextResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictPositionsFromTextResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictTimeTableRequest> __Marshaller_event_v1_PredictTimeTableRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictTimeTableRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictTimeTableResponse> __Marshaller_event_v1_PredictTimeTableResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictTimeTableResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictEventItemRequest> __Marshaller_event_v1_PredictEventItemRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictEventItemRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Event.V1.PredictEventItemResponse> __Marshaller_event_v1_PredictEventItemResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Event.V1.PredictEventItemResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Event.V1.PredictEventMaterialItemRequest, global::Event.V1.PredictEventMaterialItemResponse> __Method_PredictEventMaterialItem = new grpc::Method<global::Event.V1.PredictEventMaterialItemRequest, global::Event.V1.PredictEventMaterialItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PredictEventMaterialItem",
        __Marshaller_event_v1_PredictEventMaterialItemRequest,
        __Marshaller_event_v1_PredictEventMaterialItemResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Event.V1.PredictPositionsFromTextRequest, global::Event.V1.PredictPositionsFromTextResponse> __Method_PredictPositionsFromText = new grpc::Method<global::Event.V1.PredictPositionsFromTextRequest, global::Event.V1.PredictPositionsFromTextResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PredictPositionsFromText",
        __Marshaller_event_v1_PredictPositionsFromTextRequest,
        __Marshaller_event_v1_PredictPositionsFromTextResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Event.V1.PredictTimeTableRequest, global::Event.V1.PredictTimeTableResponse> __Method_PredictTimeTable = new grpc::Method<global::Event.V1.PredictTimeTableRequest, global::Event.V1.PredictTimeTableResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PredictTimeTable",
        __Marshaller_event_v1_PredictTimeTableRequest,
        __Marshaller_event_v1_PredictTimeTableResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Event.V1.PredictEventItemRequest, global::Event.V1.PredictEventItemResponse> __Method_PredictEventItem = new grpc::Method<global::Event.V1.PredictEventItemRequest, global::Event.V1.PredictEventItemResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PredictEventItem",
        __Marshaller_event_v1_PredictEventItemRequest,
        __Marshaller_event_v1_PredictEventItemResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Event.V1.EventMaterialReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of EventMaterialService</summary>
    [grpc::BindServiceMethod(typeof(EventMaterialService), "BindService")]
    public abstract partial class EventMaterialServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Event.V1.PredictEventMaterialItemResponse> PredictEventMaterialItem(global::Event.V1.PredictEventMaterialItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Event.V1.PredictPositionsFromTextResponse> PredictPositionsFromText(global::Event.V1.PredictPositionsFromTextRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Event.V1.PredictTimeTableResponse> PredictTimeTable(global::Event.V1.PredictTimeTableRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::Event.V1.PredictEventItemResponse> PredictEventItem(global::Event.V1.PredictEventItemRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for EventMaterialService</summary>
    public partial class EventMaterialServiceClient : grpc::ClientBase<EventMaterialServiceClient>
    {
      /// <summary>Creates a new client for EventMaterialService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public EventMaterialServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for EventMaterialService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public EventMaterialServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected EventMaterialServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected EventMaterialServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictEventMaterialItemResponse PredictEventMaterialItem(global::Event.V1.PredictEventMaterialItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictEventMaterialItem(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictEventMaterialItemResponse PredictEventMaterialItem(global::Event.V1.PredictEventMaterialItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PredictEventMaterialItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictEventMaterialItemResponse> PredictEventMaterialItemAsync(global::Event.V1.PredictEventMaterialItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictEventMaterialItemAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictEventMaterialItemResponse> PredictEventMaterialItemAsync(global::Event.V1.PredictEventMaterialItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PredictEventMaterialItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictPositionsFromTextResponse PredictPositionsFromText(global::Event.V1.PredictPositionsFromTextRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictPositionsFromText(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictPositionsFromTextResponse PredictPositionsFromText(global::Event.V1.PredictPositionsFromTextRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PredictPositionsFromText, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictPositionsFromTextResponse> PredictPositionsFromTextAsync(global::Event.V1.PredictPositionsFromTextRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictPositionsFromTextAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictPositionsFromTextResponse> PredictPositionsFromTextAsync(global::Event.V1.PredictPositionsFromTextRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PredictPositionsFromText, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictTimeTableResponse PredictTimeTable(global::Event.V1.PredictTimeTableRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictTimeTable(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictTimeTableResponse PredictTimeTable(global::Event.V1.PredictTimeTableRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PredictTimeTable, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictTimeTableResponse> PredictTimeTableAsync(global::Event.V1.PredictTimeTableRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictTimeTableAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictTimeTableResponse> PredictTimeTableAsync(global::Event.V1.PredictTimeTableRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PredictTimeTable, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictEventItemResponse PredictEventItem(global::Event.V1.PredictEventItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictEventItem(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Event.V1.PredictEventItemResponse PredictEventItem(global::Event.V1.PredictEventItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PredictEventItem, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictEventItemResponse> PredictEventItemAsync(global::Event.V1.PredictEventItemRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PredictEventItemAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Event.V1.PredictEventItemResponse> PredictEventItemAsync(global::Event.V1.PredictEventItemRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PredictEventItem, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override EventMaterialServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new EventMaterialServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(EventMaterialServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_PredictEventMaterialItem, serviceImpl.PredictEventMaterialItem)
          .AddMethod(__Method_PredictPositionsFromText, serviceImpl.PredictPositionsFromText)
          .AddMethod(__Method_PredictTimeTable, serviceImpl.PredictTimeTable)
          .AddMethod(__Method_PredictEventItem, serviceImpl.PredictEventItem).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, EventMaterialServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_PredictEventMaterialItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Event.V1.PredictEventMaterialItemRequest, global::Event.V1.PredictEventMaterialItemResponse>(serviceImpl.PredictEventMaterialItem));
      serviceBinder.AddMethod(__Method_PredictPositionsFromText, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Event.V1.PredictPositionsFromTextRequest, global::Event.V1.PredictPositionsFromTextResponse>(serviceImpl.PredictPositionsFromText));
      serviceBinder.AddMethod(__Method_PredictTimeTable, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Event.V1.PredictTimeTableRequest, global::Event.V1.PredictTimeTableResponse>(serviceImpl.PredictTimeTable));
      serviceBinder.AddMethod(__Method_PredictEventItem, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Event.V1.PredictEventItemRequest, global::Event.V1.PredictEventItemResponse>(serviceImpl.PredictEventItem));
    }

  }
}
#endregion
