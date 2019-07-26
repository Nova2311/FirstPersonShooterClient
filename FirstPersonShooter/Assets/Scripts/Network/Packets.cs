public enum DatabaseServerPackets {
    DB_UsernameExistsError,
    DB_EmailExistsError,
    DB_IncorrectLoginDetails,
    DB_ConfirmLoginDetails,
    DB_CharacterNameExists,
}

public enum ClientPackets {
    C_LoginCredentials,
    C_CreateAccountDetails,
    C_CreateCharacterDetails,
}