namespace AITLib.Constants
{
    public enum AitDateTimeType
    {
        DATE,
        DATE_TIME,
        TIME_NO_DATE,
        NO_DATE
    }

    public enum AitNotesTypesModel
    {
        SIMPLE_NOTE_MODEL,
        TREE_VIEW_MODEL,
        TRAINING_MODEL
    }

    public enum AitTrainingTypes
    {
        UNSPECIFIED,
        PUMP,
        SQUATS,
        DUMBBELL,
        BENCH,
        BIKE,
        RUN,
        WALK
    }

    /// <summary>
    /// <para>UNF - unspecified</para>
    /// <para>MWU - morning first action after wake up,</para>
    /// <para>MMT - morning first action after morning toilet,</para>
    ///	<para>MBB - morning before breakfast,</para>
    ///	<para>MAB - morning after breakfast,</para>
    ///	<para>MBL - midday before lunch,</para>
    ///	<para>MAL - midday after lunch,</para>
    ///	<para>ABD - afternoom before dinner,</para>
    ///	<para>AAD - afternoom after dinner,</para>
    ///	<para>NBB - night before bath,</para>
    ///	<para>NAB - night after bath,</para>
    ///	<para>NBS - night before sleep</para>
    /// </summary>
    public enum AitTimeOfDay
    {
        UNF,
        MWU,
        MMT,
	    MBB,
	    MAB,
	    MBL,
	    MAL,
	    ABD,
	    AAD,
	    NBB,
	    NAB,
	    NBS
    }
}
