define 'user_panel_view', ['general_view', 'config','breadcrumb_view','user_invitations_view','form_view','navbar_view'], (GeneralView, config, BreadcrumbView, UserInvitationsView, FormView, NavbarView) ->

    class UserPanelView extends GeneralView

        id: 'user-panel-container'
        initialize: ->
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
            me = @
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
            @$el.empty()
            @breadcrumb.render().append_to @$el
            $("#client-content").attr("class", "container")
            add_room_form_row = $("<div class='row span12' style='margin-left:0px;'></div>")
            @formview.$el.attr('class', 'span6 form-inline')
            @formview.$el.attr('style', 'margin-left:0px')
            @formview.render().append_to add_room_form_row
            @$el.append add_room_form_row
            add_room_form_row.append """<a href='https://s3.amazonaws.com/yackety-vs-plugin/TeamNotification_Package.vsix' class='btn btn-info inline pull-right' >Download our vs plugin</a>"""
            rooms_row = $("<div class='row'></div>")
            rooms_table = $("""<table/>""", {"class":"table table-striped"})
            rooms_table.append "<tr><th style='width:75%'>Rooms</th><th style='width:25%'></th</tr>"
            if @model.has('rooms')
                rooms = @model.get('rooms')
                for room in rooms
                    room_id = @get_field 'id', room.data
                    room_name = @get_field 'name', room.data
                    owner_id = @get_field 'owner_id', room.data
                    new_row = $("""<tr/>""")
                    new_row.append """<td><h3><a href='#/room/#{room_id}/messages'>#{@get_field('name', room.data)}</a></h3></td>"""
                    if owner_id == @model.get("user").user_id
                        new_row.append """<td><div class="btn-group pull-right">
                                            <button class="btn dropdown-toggle" data-toggle="dropdown" tabindex="-1"><i class="icon-asterisk"></i><span class="caret"></span></button>
                                            <ul class="dropdown-menu">
                                              <li><a href="#/room/#{room_id}">Manage Room</a></li>
                                              <!-- <li><a href="/github/oauth">Associate Repository</a></li> -->
                                            </ul></div></td>"""
                    else
                        td = $("<td/>")
                        btn_group = $('<div class="btn-group pull-right"/>')
                        btn_group.append """<button class="btn dropdown-toggle" data-toggle="dropdown" tabindex="-1"><i class="icon-asterisk"></i><span class="caret"></span></button>"""
                        td.append btn_group
                        ul = $('<ul class="dropdown-menu"/>')
                        btn_group.append ul
                        link = generate_unsubscribe_link(room_id,room_name,new_row)
                        ul.append link
                        new_row.append td
                    rooms_table.append new_row
            else
                rooms_table.append "<tr><td colspan='2'><p>No rooms available</p></td></tr>"
            @$el.append rooms_table
            invitations_row = $("""<div></div>""")
            @invitations.render().append_to invitations_row
            @$el.append invitations_row
            @
