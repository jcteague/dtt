define 'rooms_view', ['general_view','config', 'breadcrumb_view', 'query_view', 'room_members_view', 'links_view'], (GeneralView, config, BreadcrumbView, QueryView, RoomMembersView, LinksView) ->

    class RoomsView extends GeneralView

        id: 'rooms-container'

        initialize: ->
            @container = $("<div/>", {'class':'row-fluid'})
            @left = $("<div/>", {'class':'span6'})
            @right = $("<div/>", {'class':'span6'})
            @query_view = new QueryView(model:@model)
            @room_users_view = new RoomMembersView(model:@model)
            @model.on 'change:rooms', @render, @
            @query_view.on 'all', @propagate_event, @
            @room_users_view.on 'all', @propagate_event, @
            authToken = $.cookie("authtoken")
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true

        render: ->
            @$el.empty()
            @left.empty()
            @right.empty()
            @container.empty()
            
            if @model.has('rooms') and @model.get('rooms').length > 0
                @rooms = @model.get('rooms')
                breadcrumb_links = [
                    {name:'Home', href:'/user', rel:'BreadcrumbLink'}
                    {name:@get_field('name',@rooms[0].data), href:@rooms[0].href, rel:'active'}
                ]
                @model.set('breadcrumb', breadcrumb_links)
                @breadcrumb_view = new BreadcrumbView(model:@model)
                @breadcrumb_view.render().append_to @$el

                @container.append @left
                @container.append @right
                @$el.append @container
                
                @right.append """<a href="/github/oauth"><img class="img-polaroid" style="width: 32px; height:32px;" src="/img/github-icon.jpg" />&nbsp;Integrate with github</a></div>"""
                @right.append """<a href="/bitbucket/oauth"><img class="img-polaroid" style="width: 32px; height:32px;" src="/img/bitbucket-icon.png" />&nbsp;Integrate with bitbucket</a></div>"""
                
                @left.append @get_link('RoomMessages',@model.attributes.links)
                @render_room(room) for room in @rooms
                
                #@left.append "Add members"
                @query_view.render().append_to @left
                @room_users_view.render().append_to @$el
            @

        render_room: (room) ->
            me = @
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
                            me.trigger 'messages:display', data.server_messages
                            container.hide()
                    return false
                anchor
            link = room.links[0]
            room_key = get_field(room.data, 'room_key')
            if( room_key == '')
                p = $("<p/>")
                unsubscribe_room_link = generate_unsubscribe_link get_field(room.data,'id'),link.name, p
                p.append unsubscribe_room_link
                @left.append p
            else
                @left.append("""<p><small> Room key: #{room_key}</small></p>""")

        append_to: (parent) ->
            @$el.appendTo parent
