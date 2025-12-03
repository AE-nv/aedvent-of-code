package aock2025

data class Year2025Day03(
    private val banks: List<BatteryBank>
) {
    constructor(input: String) : this(input.replace("\r", "").lines().map { BatteryBank.parse(it) })

    fun partOne() = banks.sumOf { bank -> bank.maxJoltage(2) }
    fun partTwo() = banks.sumOf { bank -> bank.maxJoltage(12) }
}

data class BatteryBank(
    private val batteries: List<Int>
) {
    companion object {
        fun parse(bankAsString: String) = BatteryBank(bankAsString.map { it.digitToInt() })

        private fun maxJoltage(
            batteries: List<Int>,
            turnOnCount: Int
        ): String {
            val selectedBattery = batteries.subList(0, batteries.size - turnOnCount + 1).max()

            if (turnOnCount == 1) {
                return selectedBattery.toString()
            }

            val selectedBatteryIndex = batteries.indexOfFirst { it == selectedBattery }
            val remainingSelectableBatteries = batteries.subList(selectedBatteryIndex + 1, batteries.size)

            return selectedBattery.toString() + maxJoltage(
                remainingSelectableBatteries,
                turnOnCount - 1
            )
        }
    }

    fun maxJoltage(turnOnCount: Int) = maxJoltage(batteries, turnOnCount).toLong()

}