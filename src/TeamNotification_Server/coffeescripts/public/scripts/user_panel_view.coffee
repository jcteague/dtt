define 'user_panel_view', ['general_view', 'config','navbar_view','breadcrumb_view','user_invitations_view','form_view'], (GeneralView, config, NavbarView, BreadcrumbView, UserInvitationsView, FormView) ->

    class UserPanelView extends GeneralView

        id: 'user-panel-container'
        initialize: ->
            @navbar = new NavbarView(model:@model)
            breadcrumb_links = [
                {name:'Home', href:'/user', rel:'active'}
            ]
            @model.set('breadcrumb', breadcrumb_links)
            @formview = new FormView(model:@get_model_for('add_room'))
            @breadcrumb = new BreadcrumbView(model:@model)
            @invitations = new UserInvitationsView(model:@model)
        render: ->
            get_field = (field_name, data)->
                for field in data
                    if(field.name == field_name)
                        return field.value
            @$el.empty()
            $("#client-content").attr("class", "container")
            @navbar.render().append_to @$el
            @breadcrumb.render().append_to @$el
            add_room_form_row = $("<div class='row'></div>")
            add_room_form_row.append "<h1>New Room</h1>"
            @formview.render().append_to add_room_form_row
            @$el.append add_room_form_row
            rooms_row = $("<div class='row'></div>")
            rooms_row.append '<h1 class="span12" >Your rooms</h1>'
            if @model.has('rooms')
                rooms = @model.get('rooms')
                for room in rooms
                    rooms_row.append """<div class='span4 hero-unit'><a href='#/room/#{get_field('id', room.data)}'><h2>#{get_field('name', room.data)}</h2></a><br/><a href='#/room/#{get_field('id', room.data)}/messages'>Go to room</a></div>"""
            else
                rooms_row.append """No rooms available at this moment"""
            @$el.append rooms_row
            invitations_row = $("""<div class='row' class="span12"></div>""")
            invitations_row.append """<h1 class="span12">Pending Invitations</h1>"""
            @invitations.render().append_to invitations_row
            @$el.append invitations_row
            @
