define 'user_panel_view', ['general_view', 'config','navbar_view','breadcrumb_view','user_invitations_view','form_view'], (GeneralView, config, NavbarView, BreadcrumbView, UserInvitationsView, FormView) ->

    class UserPanelView extends GeneralView

        id: 'user-panel-container'
        initialize: ->
            @navbar = new NavbarView(model:@model)
            breadcrumb_links = [
                {name:'Home', href:'/user', rel:'active'}
            ]
            @model.set('breadcrumb', breadcrumb_links)
            @formview = new FormView(model:@model)
            @breadcrumb = new BreadcrumbView(model:@model)
            @invitations = new UserInvitationsView(model:@model)
            @invitations.on 'all', @propagate_event, @
            authToken = $.cookie("authtoken")
            $.ajaxSetup
                beforeSend: (jqXHR) ->
                    jqXHR.setRequestHeader('Authorization', authToken )
                    jqXHR.withCredentials = true
        render: ->
            @$el.empty()
            $("#client-content").attr("class", "container")
            @navbar.render().append_to @$el
            @breadcrumb.render().append_to @$el
            add_room_form_row = $("<div class='row span'></div>")
            @formview.$el.attr('class', 'form-inline')
            @formview.render().append_to add_room_form_row
            @$el.append add_room_form_row
            rooms_row = $("<div class='row'></div>")
            owned_rooms_table = $('<div class="span7 well"><b>Owned Rooms</b>&nbsp;<a href="/github/oauth">Associate Repository</a></div>')
            other_rooms_table = $("<div class='span4 well'><b>Other Rooms</b></div>")
            rooms_row.append owned_rooms_table
            rooms_row.append other_rooms_table
            owns_rooms = false
            has_other_rooms = false
            if @model.has('rooms')
                rooms = @model.get('rooms')
                for room in rooms
                    room_id = @get_field 'id', room.data
                    owner_id = @get_field 'owner_id', room.data
                    if owner_id == @model.get("user").user_id
                        owns_rooms = true
                        owned_rooms_table.append """<h3><a href='#/room/#{room_id}/messages'>#{@get_field('name', room.data)}</a>&nbsp;<small><a href='#/room/#{room_id}/'>Manage room</a></small></h3><h4><small>Room Key: #{@get_field('room_key', room.data)}</small></h4>"""
                    else
                        has_other_rooms = true
                        other_rooms_table.append """<h3><a href='#/room/#{room_id}/messages'>#{@get_field('name', room.data)}</a>&nbsp;-&nbsp;<small><a href='#'>Unsubscribe</a></small></h3>"""
            else
                owned_rooms_table.append """<p>No rooms available</p>"""
                other_rooms_table.append """<p>No rooms available</p>"""
            if !owns_rooms
                owned_rooms_table.append """<p>No rooms available</p>"""
            if !has_other_rooms
                other_rooms_table.append """<p>No rooms available</p>"""
            @$el.append rooms_row
            invitations_row = $("""<div class='row' class="span12"></div>""")
            @invitations.render().append_to invitations_row
            @$el.append invitations_row
            @
