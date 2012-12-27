define 'rooms_view', ['general_view','config', 'breadcrumb_view','navbar_view', 'query_view'], (GeneralView, config, BreadcrumbView, NavbarView, QueryView) ->

    class RoomsView extends GeneralView

        id: 'rooms-container'

        initialize: ->
            @navbar_view = new NavbarView(model:@model)
            @query_view = new QueryView(model:@model)
            @model.on 'change:rooms', @render, @
            @query_view.on 'all', @propagate_event, @
            authToken = $.cookie("authtoken")
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true

        render: ->
            @$el.empty()
            @navbar_view.render().append_to @$el
            if @model.has('rooms') and @model.get('rooms').length > 0
                @rooms = @model.get('rooms')
                breadcrumb_links = [
                    {name:'Home', href:'#/user', rel:'BreadcrumbLink'}
                    {name:@get_field('name',@rooms[0].data), href:@rooms[0].href, rel:'active'}
                ]
                @model.set('breadcrumb', breadcrumb_links)
                @breadcrumb_view = new BreadcrumbView(model:@model)
                @breadcrumb_view.render().append_to @$el
                
                @render_room(room) for room in @rooms
                @$el.append "<h1>Add members</h1>"
                @query_view.render().append_to @$el
            @

        render_room: (room) ->
            get_field = (data, name) ->
                for field in data
                    if(field.name == name)
                        return field.value
                return ""
            generate_unsubscribe_link = (room_id, room_name, container) ->
                anchor = $('<a href="#">Unsubscribe</a>')
                anchor.bind 'click',() ->
                    del = confirm("Are you sure you want to leave room '#{room_name}'?")
                    if(del)
                        $.post "#{config.api.url}/room/#{room_id}/unsubscribe", (data) ->
                            $("#server-response-container").html(data.server_messages[0])
                            container.hide()
                    return false
                anchor
            link = room.links[0]
            room_key = get_field(room.data, 'room_key')
            if( room_key == '')
                p = $("<p/>")
                unsubscribe_room_link = generate_unsubscribe_link get_field(room.data,'id'),link.name, p
                p.append unsubscribe_room_link
                @$el.append p
            else
                @$el.append("""<p><small> Room key: #{room_key}</small></p>""")
                

        append_to: (parent) ->
            @$el.appendTo parent
