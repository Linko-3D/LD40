public class TeaController : ItemController {

    public override void OnConsumedBy(PrincessCakeController controller) {
        base.OnConsumedBy(controller);

        controller.Model.DrinkTea();
    }

}
