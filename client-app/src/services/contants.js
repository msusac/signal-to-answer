export const GameType = {
    SOLO: { id: 1, name: "Solo Game" },
    PUBLIC: { id: 2, name: "Public Game" },
    PRIVATE: { id: 3, name: "Private Game" }
}

export const GroupType = {
    OFFLINE: { id: 1, name: "Offline", mark: "OFFLINE"},
    MAIN_LOBBY:  { id: 2, name: "Main Lobby", mark: "MAIN_LOBBY"},
    SOLO_LOBBY: { id: 3, name: "Solo Lobby", mark: "SOLO_LOBBY"},
    PUBLIC_LOBBY: { id: 4, name: "Public Lobby", mark: "PUBLIC_LOBBY"},
    PRIVATE_LOBBY: { id: 5, name: "Private Lobby", mark: "PRIVATE_LOBBY"},
    INVITE_LOBBY: { id: 6, name: "Invite Lobby", mark: "INVITE_LOBBY" },
    IN_GAME_SOLO: { id: 7, name: "In-Game Solo", mark: "IN_GAME_SOLO"},
    IN_GAME_PUBLIC: { id: 8, name: "In-Game Public", mark: "IN_GAME_PUBLIC"},
    IN_GAME_PRIVATE: { id: 9, name: "In-Game Private", mark: "IN_GAME_PRIVATE"}
}

export const Roles = {
    GUEST: { id: 1, name: "Guest", mark: "GUEST" },
    USER: { id: 2, name: "User", mark: "USER" }
}

export const QuestionCategories = {
    GENERAL_KNOWLEDGE: { id: 1, name: "General knowledge" },
    LITERATURE: { id: 2, name: "Literature" },
    HISTORY: { id: 3, name: "History" },
    SCIENCE: { id: 4, name: "Science" },
    MUSIC: { id: 5, name: "Music" },
    MOVIES: { id: 6, name: "Movies" },
    SOCIETY_CULTURE: { id: 7, name: "Society and culture" },
    SPORT_LEISURE: { id: 8, name: "Sport and leisure" },
    GEOGRAPHY: { id: 9, name: "Geography" },
    FOOD_DRINK: { id: 10, name: "Food and drink" }
}

export const QuestionDifficulties = {
    DEFAULT: { id: 0, name: "Default" },
    EASY: { id: 1, name: "Easy" },
    MEDIUM: { id: 2, name: "Medium" },
    HARD: { id: 3, name: "Hard" }
}
