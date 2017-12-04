public class TeaController : ItemController, IController {

    public override void OnConsumedBy(PrincessCakeController controller) {
        base.OnConsumedBy(controller);

        controller.Model.DrinkTea();
    }

}
