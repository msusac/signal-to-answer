export const AnswerStatus = {
    CORRECT: 1,
    INCORRECT: 2
}

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
    ARTS_LITERATURE: { id: 1, name: "Arts & Literature" },
    FILM_TV: { id: 2, name: "Film & TV" },
    FOOD_DRINK: { id: 3, name: "Food & Drink" },
    GENERAL_KNOWLEDGE: { id: 4, name: "General knowledge" },
    GEOGRAPHY: { id: 5, name: "Geography" },
    HISTORY: { id: 6, name: "History" },
    MUSIC: { id: 7, name: "Music" },
    SCIENCE: { id: 8, name: "Science" },
    SOCIETY_CULTURE: { id: 9, name: "Society & Culture" },
    SPORT_LEISURE: { id: 10, name: "Sport & Leisure" },
}

export const QuestionDifficulties = {
    DEFAULT: { id: 0, name: "Default" },
    EASY: { id: 1, name: "Easy" },
    MEDIUM: { id: 2, name: "Medium" },
    HARD: { id: 3, name: "Hard" }
}

export const WinnerStatus = {
    WIN: { id: 1, name: "Win" },
    LOSS: { id: 2, name: "Loss" },
    DRAW: { id: 3, name: "Draw" }
}

export const ReplayStatus = {
    WANTS_TO_REPLAY: 1,
    DOES_NOT_WANT_TO_REPLAY: 2
}