namespace SpartaFarming.Core
{
    /// <summary>
    /// 스태미나 시스템을 사용하는 엔티티를 위한 인터페이스입니다.
    /// </summary>
    public interface IStaminaUser
    {
        /// <summary>
        /// 현재 스태미나
        /// </summary>
        float CurrentStamina { get; }

        /// <summary>
        /// 최대 스태미나
        /// </summary>
        float MaxStamina { get; }

        /// <summary>
        /// 스태미나 회복 속도(초당)
        /// </summary>
        float StaminaRecoveryRate { get; }

        /// <summary>
        /// 스태미나를 소비합니다.
        /// </summary>
        /// <param name="amount">소비할 스태미나 양</param>
        /// <returns>실제로 소비된 스태미나 양</returns>
        float UseStamina(float amount);

        /// <summary>
        /// 스태미나를 회복합니다.
        /// </summary>
        /// <param name="amount">회복할 스태미나 양</param>
        /// <returns>실제로 회복된 스태미나 양</returns>
        float RecoverStamina(float amount);

        /// <summary>
        /// 특정 행동에 필요한 스태미나가 충분한지 확인합니다.
        /// </summary>
        /// <param name="requiredAmount">필요한 스태미나 양</param>
        /// <returns>스태미나가 충분한지 여부</returns>
        bool HasEnoughStamina(float requiredAmount);
    }
} 