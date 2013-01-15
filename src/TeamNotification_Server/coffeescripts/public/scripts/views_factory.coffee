define 'views_factory', ['messages_view', 'links_view', 'rooms_view', 'form_view', 'query_view', 'chat_room_view', 'login_view', 'user_edit_view', 'user_invitations_view','room_members_view','root_view', 'user_panel_view','repository_selection_view'], (MessagesView, LinksView, RoomsView, FormView, QueryView, ChatRoomView, LoginView, UserEditView, UserInvitationsView, RoomMembersView, RootView, UserPanelView, RepositorySelectionView) ->
    class ViewsFactory
        registry:
            root: RootView
            messages: MessagesView
            #rooms: RoomsView
            template: FormView
            queries: QueryView
            #links: LinksView
            members: RoomMembersView
        pages:
            login: {pattern:/user\/login/g, view: LoginView}
            user_edit: {pattern:/user\/\d+\/edit/g, view: UserEditView}
            chat_room: { pattern:/room\/\d+\/messages/g, view: ChatRoomView }
            room: {pattern:/room\/\d+/g, view: RoomsView}
            user_invitations: { pattern:/user\/\d+\/invitations/g, view: UserInvitationsView }
            room_invitations: { pattern:/room\/\d+\/invitations/g, view: UserInvitationsView }
            user_panel: {pattern:/user\/\d+/g, view: UserPanelView}
            repositories_selection: {pattern:/github\/repositories\/*/g, view: RepositorySelectionView}
        get_for: (collection_model) ->
            collection_data = collection_model.attributes
            for page, props of @pages
                if props.pattern.test(collection_data.href) or props.pattern.test(collection_data.href)
                    return [(new props.view(model:collection_model))] 
            keys = _.keys(collection_data)
            (new view(model: collection_model) for key, view of @registry when contains(keys, key))

        contains = (arr, element) ->
            arr.indexOf(element) != -1;

