namespace Shared.Enums;

public enum TechProcessReadonlySearchOptions
{
	Base,
	SerialNumber,
	Title,
	Developer
}

public enum TechProcessReadonlyOrderOptions
{
	Base,
	SerialNumber,
	Title,
	Rate,
	Developer
}

public enum CompletedDeveloperTechProcessesOrderOptions
{
	Base,
	Date,
	SerialNumber,
	Title
}

public enum TechProcessStatusesForDeveloper
{
	InWork = 4,
	Paused = 5,
	OnApproval = 6,
	ForIssuance = 8
}

public enum TechProcessStatusesForDirector
{
	NotInWork = 2,
	ReturnForRework = 7,
	Completed = 11
}

public enum TechProcessStatusesForArchive
{
	Issued = 9,
}

public enum IssuedTechProcessOrderOptions
{
	Base,
	SerialNumber,
	Title,
	Material,
	BlankType,
	Developer
}

public enum IssuedDuplicateTechProcessOrderOptions
{
	Base,
	DateOfIssue,
	FFL,
	ProfessionNumber,
	Subdivision
}

public enum IssuedTechProcessesFromTechnologistOrderOptions
{
	Base,
	DateOfIssue,
	SerialNumber,
	Title,
	FFL,
	ProfessionNumber,
	Subdivision
}

public enum TechProcessDevelopmentStagesOrderOptions
{
	Base,
	StatusDate,
	Status
}

public enum TechProcessStatusType
{
	Base,
	Developer,
	Supervisor,
	Archive
}
