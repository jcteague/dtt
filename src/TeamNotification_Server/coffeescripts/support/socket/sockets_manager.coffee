methods = {}
list_of_listeners = {}

methods.setup_message_transmission = (io, listener_name, redis_subscriber) ->
    namespace_io = io.of(listener_name)
    room_channel = listener_name
    if (redis_subscriber?)
        redis_subscriber.subscribe(room_channel)
        redis_subscriber.on "message", (channel, message) ->
            if(channel == listener_name)
                namespace_io.send(message)
        redis_subscriber.on "disconnect", (channel, message) ->
            delete list_of_listeners[channel]
            console.log "Disconnected from listener #{channel}"

methods.set_socket_events = (io, listener_name, redis_subscriber = null) ->
    unless methods.is_listener_registered(listener_name)
        list_of_listeners[listener_name] = true
        methods.setup_message_transmission(io, listener_name, redis_subscriber)

methods.is_listener_registered = (listener_name) ->
    list_of_listeners[listener_name]?
    
module.exports =
    set_socket_events: methods.set_socket_events
    is_listener_registered: methods.is_listener_registered
    setup_message_transmission: methods.setup_message_transmission
